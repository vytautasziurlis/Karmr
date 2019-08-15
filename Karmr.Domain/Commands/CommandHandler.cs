using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Karmr.Common.Contracts;
using Karmr.Domain.Entities;
using Karmr.Common.Helpers;
using Karmr.Common.Infrastructure;

namespace Karmr.Domain.Commands
{
    public sealed class CommandHandler : ICommandHandler
    {
        private readonly BindingFlags BindingFlags = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private readonly IClock clock;

        private readonly IEventRepository repository;

        private readonly IEnumerable<Type> entityTypes;

        private readonly Dictionary<Type, Type> commandEntities = new Dictionary<Type, Type>();

        private readonly IDenormalizerHandler denormalizerHandler;

        public CommandHandler(IClock clock, IEventRepository repository, IEnumerable<Type> entityTypes, IDenormalizerHandler denormalizerHandler)
        {
            this.clock = clock;
            this.repository = repository;
            this.entityTypes = entityTypes;
            this.denormalizerHandler = denormalizerHandler;
        }

        public void Handle(ICommand command)
        {
            command.Validate();
            var entity = this.GetEntityInstance(command.GetType(), command.EntityKey);
            entity.Handle(command);

            var uncommittedEvents = entity.GetUncommittedEvents();
            var sequenceNumber = entity.Events.Count - uncommittedEvents.Count;
            foreach (var @event in uncommittedEvents)
            {
                this.repository.Save(entity.GetType(), command.EntityKey, @event, sequenceNumber);
                sequenceNumber++;
            }

            // send events to denormalizers
            this.denormalizerHandler.Handle(uncommittedEvents);
        }

        private Entity GetEntityInstance(Type commandType, Guid entityKey)
        {
            var entityType = this.GetEntityType(commandType);
            var @params = new object[] { this.clock, this.repository.Get(entityKey) };

            return Activator.CreateInstance(entityType, this.BindingFlags, null, @params, null) as Entity;
        }

        private Type GetEntityType(Type commandType)
        {
            if (!this.commandEntities.ContainsKey(commandType))
            {
                var matchingEntityTypes = this.entityTypes
                    .Where(t => t.IsSubclassOf(typeof(Entity))).ToList()
                    .Where(t => t.GetMethodBySignature(typeof(void), new[] { commandType }, this.BindingFlags) != null)
                    .ToList();

                if (matchingEntityTypes.Count != 1)
                {
                    throw new UnhandledCommandException(string.Format("Expected exactly one entity to handle command {0}, found {1} instead.",
                        commandType,
                        matchingEntityTypes.Count));
                }

                this.commandEntities.Add(commandType, matchingEntityTypes.First());
            }
            return this.commandEntities[commandType];
        }
    }
}