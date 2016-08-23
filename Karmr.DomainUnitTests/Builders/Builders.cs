using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using System;

namespace Karmr.DomainUnitTests.Builders
{
    internal static class Builder
    {
        internal static AggregateBuilder<T> Aggregate<T>() where T: Aggregate
        {
            return new AggregateBuilder<T>();
        }

        internal static CommandBuilder<T> Command<T>() where T : Command
        {
            return new CommandBuilder<T>();
        }
    }

    internal class AggregateBuilder<T> where T: Aggregate
    {
        private T aggregate;

        internal AggregateBuilder()
        {
            this.aggregate = (T)Activator.CreateInstance(typeof(T));
        }

        internal T Build()
        {
            return this.aggregate;
        }
    }

    internal class CommandBuilder<T> where T : Command
    {
        protected T command;

        internal CommandBuilder()
        {
            this.command = (T)Activator.CreateInstance(typeof(T));
        }

        //internal CommandBuilder<T> With(Action<T> action)
        //{
            
        //}

        internal T Build()
        {
            return this.command;
        }
    }
}