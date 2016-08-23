using System;
using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Karmr.Contracts.Commands;

namespace Karmr.DomainUnitTests.Entities
{
    public class AggregateTests
    {
        [Test]
        public void NewAggregateHasEmptyCommandList()
        {
            var subject = new ConcreteAggregate(null);
            Assert.IsEmpty(subject.GetCommands());
        }

        [Test]
        public void HandlingCommandUpdatesCommandsList()
        {
            var subject = new ConcreteAggregate(x => true);
            var command1 = new Command();
            var commands = this.HandleCommand(subject, command1);
            Assert.AreEqual(1, commands.Count());
            Assert.AreSame(command1, commands.First());

            var command2 = new Command();
            commands = this.HandleCommand(subject, command2);
            Assert.AreEqual(2, commands.Count());
            Assert.AreSame(command1, commands.First());
            Assert.AreSame(command2, commands.Last());
        }

        [Test]
        public void HandlingFalsyCommandDoesNotChangeCommandsList()
        {
            var subject = new ConcreteAggregate(x => false);
            subject.Handle(new Command());
            Assert.IsEmpty(subject.GetCommands());
        }

        [Test]
        public void HandlingExceptionyCommandThrowsException()
        {
            var subject = new ConcreteAggregate(x => { throw new Exception(); });
            Assert.Throws<Exception>(() => subject.Handle(new Command()));
        }

        private IEnumerable<ICommand> HandleCommand(Aggregate aggregate, Command command)
        {
            aggregate.Handle(command);
            return aggregate.GetCommands();
        }

        private class ConcreteAggregate : Aggregate
        {
            private Func<Command, bool> handleFunc { get; set; }

            public ConcreteAggregate(Func<Command, bool> func)
            {
                this.handleFunc = func;
            }

            internal bool Handle(Command command)
            {
                return handleFunc.Invoke(command as Command);
            }
        }
    }
}