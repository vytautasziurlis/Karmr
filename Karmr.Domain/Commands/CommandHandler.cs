using System;
using System.Collections.Generic;
using System.Linq;

namespace Karmr.Domain.Commands
{
    using System.Reflection;

    using Karmr.Contracts;
    using Karmr.Domain.Entities;
    using Helpers;
    using Infrastructure;

    public sealed class CommandHandler
    {
        private readonly BindingFlags BindingFlags = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private readonly IClock clock;

        private readonly IEventRepository repository;

        private readonly IEnumerable<Type> entityTypes;

        private readonly Dictionary<Type, Type> commandEntities = new Dictionary<Type, Type>();

        public CommandHandler(IClock clock, IEventRepository repository, IEnumerable<Type> entityTypes)
        {
            this.clock = clock;
            this.repository = repository;
            this.entityTypes = entityTypes;
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
                this.repository.Save(@event, sequenceNumber);
                sequenceNumber++;
            }
        }

        private Entity GetEntityInstance(Type commandType, Guid entityKey)
        {
            var entityType = this.GetEntityType(commandType);
            var @params = new object[] { this.clock, this.repository.Get(entityType, entityKey) };

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
                        this.entityTypes.Count()));
                }

                this.commandEntities.Add(commandType, matchingEntityTypes.First());
            }
            return this.commandEntities[commandType];
        }
    }
}