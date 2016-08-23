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

    public sealed class CommandHandler
    {
        private readonly BindingFlags BindingFlags = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private readonly ICommandRepository repository;

        private readonly Dictionary<Type, Type> commandAggregates = new Dictionary<Type, Type>();

        public CommandHandler(ICommandRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(ICommand command)
        {
            var aggregate = this.GetAggregateInstance(command.GetType(), null);
            this.Handle(aggregate, command);
        }

        public void Handle(ICommand command, Guid entityKey)
        {
            var aggregate = this.GetAggregateInstance(command.GetType(), entityKey);
            this.Handle(aggregate, command);
        }

        private void Handle(Aggregate aggregate, ICommand command)
        {
            aggregate.Handle(command);
            this.repository.Save(command);
        }

        private Aggregate GetAggregateInstance(Type commandType, Guid? entityKey)
        {
            var aggregateType = this.GetAggregateType(commandType);
            var @params = new object[] { };
            if (entityKey.HasValue)
            {
                @params = new object[] { this.repository.Get(aggregateType, entityKey.Value) };
            }
            
            return Activator.CreateInstance(aggregateType, this.BindingFlags, null, @params, null) as Aggregate;
        }

        private Type GetAggregateType(Type commandType)
        {
            if (!this.commandAggregates.ContainsKey(commandType))
            {
                var aggregateType = Assembly.GetAssembly(typeof(Aggregate)).GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Aggregate))).ToList()
                    .FirstOrDefault(t => t.GetMethodBySignature(typeof(void), new[] { commandType }, this.BindingFlags) != null);

                if (aggregateType == null)
                {
                    throw new Exception(string.Format("Could not find aggregate to handle command {0}", commandType));
                }

                this.commandAggregates.Add(commandType, aggregateType);
            }
            return this.commandAggregates[commandType];
        }
    }
}