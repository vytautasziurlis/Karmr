using System;
using System.Collections.Generic;
using System.Linq;

namespace Karmr.Domain.Commands
{
    using System.Reflection;

    using Karmr.Contracts;
    using Karmr.Contracts.Commands;
    using Karmr.Domain.Entities;
    using Helpers;
    using Infrastructure;

    public sealed class CommandHandler
    {
        private readonly BindingFlags BindingFlags = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private readonly ICommandRepository repository;

        private readonly IEnumerable<Type> entityTypes;

        private readonly Dictionary<Type, Type> commandEntities = new Dictionary<Type, Type>();

        public CommandHandler(ICommandRepository repository, IEnumerable<Type> entityTypes)
        {
            this.repository = repository;
            this.entityTypes = entityTypes;
        }

        public void Handle(ICommand command)
        {
            command.Validate();
            var entity = this.GetEntityInstance(command.GetType(), command.EntityKey);
            entity.Handle(command);
            this.repository.Save(command);
        }

        private Entity GetEntityInstance(Type commandType, Guid entityKey)
        {
            var entityType = this.GetEntityType(commandType);
            var @params = new object[] { this.repository.Get(entityType, entityKey) };

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