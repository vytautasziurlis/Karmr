using System;
using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Karmr.Contracts.Commands;

namespace Karmr.DomainUnitTests.Aggregates
{
    public class AggregateTests
    {
        [Test]
        public void NewAggregateHasEmptyCommandList()
        {
            var subject = this.GetSubject(null);
            Assert.IsEmpty(subject.GetCommands());
        }

        [Test]
        public void HandlingCommandUpdatesCommands()
        {
            var subject = this.GetSubject(x => { });
            var command1 = new ConcreteCommand();
            var commands = this.HandleCommand(subject, command1).ToList();
            Assert.AreEqual(1, commands.Count);
            Assert.AreSame(command1, commands.First());

            var command2 = new ConcreteCommand();
            commands = this.HandleCommand(subject, command2).ToList();
            Assert.AreEqual(2, commands.Count);
            Assert.AreSame(command1, commands.First());
            Assert.AreSame(command2, commands.Last());
        }

        [Test]
        public void HandlingFalsyCommandDoesNotChangeState()
        {
            var subject = this.GetSubject(x => { });
            subject.Handle(new ConcreteCommand());
            Assert.IsEmpty(subject.GetCommands());
        }

        [Test]
        public void HandlingExceptionyCommandThrowsException()
        {
            var subject = this.GetSubject(x => { throw new Exception(); });
            Assert.Throws<Exception>(() => subject.Handle(new ConcreteCommand()));
        }

        private IEnumerable<ICommand> HandleCommand(Aggregate aggregate, Command command)
        {
            aggregate.Handle(command);
            return aggregate.GetCommands();
        }

        private ConcreteAggregate GetSubject(Action<Command> func)
        {
            return new ConcreteAggregate(func);
        }

        private class ConcreteAggregate : Aggregate
        {
            private Action<Command> HandleFunc { get; }

            public ConcreteAggregate(Action<Command> func)
            {
                this.HandleFunc = func;
            }

            internal void Handle(ConcreteCommand command)
            {
                this.HandleFunc.Invoke(command);
            }
        }

        private class ConcreteCommand : Command { }
    }
}