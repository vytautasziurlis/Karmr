using System;
using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Karmr.Contracts.Commands;

namespace Karmr.DomainUnitTests.Entities
{
    public class EntityTests
    {
        [Test]
        public void NewEntityHasEmptyEventList()
        {
            var subject = this.GetSubject(null);
            Assert.IsEmpty(subject.Events);
        }

        [Test]
        public void HandlingCommandUpdatesEvents()
        {
            var subject = this.GetSubject(x => { });
            var command1 = new ConcreteCommand();
            subject.Handle(command1);
            Assert.AreEqual(1, subject.Events);
            Assert.AreSame(command1, commands.First());

            var command2 = new ConcreteCommand();
            subject.Handle(command2);
            Assert.AreEqual(2, subject.Events.Count);
            Assert.AreSame(command1, commands.First());
            Assert.AreSame(command2, commands.Last());
        }

        [Test]
        public void HandlingFalsyCommandDoesNotChangeState()
        {
            var subject = this.GetSubject(x => { });
            subject.Handle(new ConcreteCommand());
            Assert.IsEmpty(subject.Events);
        }

        [Test]
        public void HandlingExceptionyCommandThrowsException()
        {
            var subject = this.GetSubject(x => { throw new Exception(); });
            Assert.Throws<Exception>(() => subject.Handle(new ConcreteCommand()));
        }

        private ConcreteEntity GetSubject(Action<Command> func)
        {
            return new ConcreteEntity(func);
        }

        private class ConcreteEntity : Entity
        {
            private Action<Command> HandleFunc { get; }

            public ConcreteEntity(Action<Command> func) : base(new List<ICommand>())
            {
                this.HandleFunc = func;
            }

            internal void Handle(ConcreteCommand command)
            {
                this.HandleFunc.Invoke(command);
            }
        }

        private class ConcreteCommand : Command
        {
            internal ConcreteCommand() : base(Guid.Empty)
            {
            }
        }
    }
}