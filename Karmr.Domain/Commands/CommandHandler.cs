using System;
using System.Collections.Generic;
using System.Linq;
using Karmr.Domain.Denormalizers;

namespace Karmr.Domain.Commands
{
    using System.Reflection;

    using Karmr.Common.Contracts;
    using Karmr.Domain.Entities;
    using Karmr.Common.Helpers;
    using Karmr.Common.Infrastructure;

    public sealed class CommandHandler
    {
        private readonly BindingFlags BindingFlags = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private readonly IClock clock;

        private readonly IEventRepository repository;

        private readonly IDenormalizerRepository denormalizerRepository;

        private readonly IEnumerable<Type> entityTypes;

        private readonly Dictionary<Type, Type> commandEntities = new Dictionary<Type, Type>();

        private readonly IEnumerable<Type> denormalizerTypes;

        private readonly Dictionary<Type, Type> eventDenormalizers = new Dictionary<Type, Type>();

        public CommandHandler(IClock clock, IEventRepository repository, IDenormalizerRepository denormalizerRepository, IEnumerable<Type> entityTypes, IEnumerable<Type> denormalizerTypes)
        {
            this.clock = clock;
            this.repository = repository;
            this.denormalizerRepository = denormalizerRepository;
            this.entityTypes = entityTypes;
            this.denormalizerTypes = denormalizerTypes;
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
            foreach (var @event in uncommittedEvents)
            {
                var denormalizer = this.GetDenormalizerInstance(@event.GetType());
                denormalizer.Apply(@event);
            }
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

        private Denormalizer GetDenormalizerInstance(Type eventType)
        {
            var denormalizerType = this.GetDenormalizerType(eventType);
            var @params = new object[] { this.denormalizerRepository };

            return Activator.CreateInstance(denormalizerType, this.BindingFlags, null, @params, null) as Denormalizer;
        }

        private Type GetDenormalizerType(Type eventType)
        {
            if (!this.eventDenormalizers.ContainsKey(eventType))
            {
                var matchingDenormalizerTypes = this.denormalizerTypes
                    .Where(t => t.IsSubclassOf(typeof(Denormalizer))).ToList()
                    .Where(t => t.GetMethodBySignature(typeof(void), new[] { eventType }, this.BindingFlags) != null)
                    .ToList();

                if (matchingDenormalizerTypes.Count != 1)
                {
                    throw new UnhandledEventException(string.Format("Expected exactly one denormalizer to handle event {0}, found {1} instead.",
                        eventType,
                        matchingDenormalizerTypes.Count));
                }

                this.eventDenormalizers.Add(eventType, matchingDenormalizerTypes.First());
            }
            return this.eventDenormalizers[eventType];
        }
    }
}