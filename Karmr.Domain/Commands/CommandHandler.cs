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

        private readonly IEnumerable<Type> aggregateTypes;

        private readonly Dictionary<Type, Type> commandAggregates = new Dictionary<Type, Type>();

        public CommandHandler(ICommandRepository repository, IEnumerable<Type> aggregateTypes)
        {
            this.repository = repository;
            this.aggregateTypes = aggregateTypes;
        }

        public void Handle(ICommand command)
        {
            var aggregate = this.GetAggregateInstance(command.GetType(), command.EntityKey);
            this.Handle(aggregate, command);
        }

        private void Handle(Aggregate aggregate, ICommand command)
        {
            aggregate.Handle(command);
            this.repository.Save(command);
        }

        private Aggregate GetAggregateInstance(Type commandType, Guid entityKey)
        {
            var aggregateType = this.GetAggregateType(commandType);
            var @params = new object[] { this.repository.Get(aggregateType, entityKey) };

            return Activator.CreateInstance(aggregateType, this.BindingFlags, null, @params, null) as Aggregate;
        }

        private Type GetAggregateType(Type commandType)
        {
            if (!this.commandAggregates.ContainsKey(commandType))
            {
                var mathingAggregateTypes = this.aggregateTypes
                    .Where(t => t.IsSubclassOf(typeof(Aggregate))).ToList()
                    .Where(t => t.GetMethodBySignature(typeof(void), new[] { commandType }, this.BindingFlags) != null)
                    .ToList();

                if (mathingAggregateTypes.Count != 1)
                {
                    throw new UnhandledCommandException(string.Format("Excepted exatly one aggregate to handle command {0}, found {1} instead.",
                        commandType,
                        this.aggregateTypes.Count()));
                }

                this.commandAggregates.Add(commandType, mathingAggregateTypes.First());
            }
            return this.commandAggregates[commandType];
        }
    }
}