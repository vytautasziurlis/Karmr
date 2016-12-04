using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using System;

namespace Karmr.DomainUnitTests.Builders
{
    using Builders;
    using Karmr.Contracts;

    using Moq;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal static class Builder
    {
        //internal static AggregateBuilder<T> Aggregate<T>() where T : Aggregate
        //{
        //    return new AggregateBuilder<T>();
        //}

        internal static CommandBuilder<T> Command<T>() where T : Command
        {
            return new CommandBuilder<T>();
        }
    }

    //internal class AggregateBuilder<T> where T: Aggregate
    //{
    //    private T aggregate;

    //    internal AggregateBuilder()
    //    {
    //        this.aggregate = (T)Activator.CreateInstance(typeof(T), Mock.Of<ICommandRepository>());
    //    }

    //    internal T Build()
    //    {
    //        return this.aggregate;
    //    }
    //}

    internal class CommandBuilder<T> where T : Command
    {
        protected T command;

        internal CommandBuilder()
        {
            this.command = (T)Activator.CreateInstance(typeof(T));

            if (this.command is CreateListingCommand)
            {
                this.With(x => (x as CreateListingCommand).Description = "Description");
            }
        }

        internal CommandBuilder<T> With(Action<T> action)
        {
            action(this.command);
            return this;
        }

        internal T Build()
        {
            return this.command;
        }
    }
}