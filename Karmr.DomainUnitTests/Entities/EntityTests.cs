using System;
using Karmr.Domain.Commands;
using Karmr.Domain.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Karmr.DomainUnitTests.Entities
{
    using Karmr.Domain.Events;

    public class EntityTests
    {
        [Test]
        public void NewEntityHasEmptyEventList()
        {
            var subject = this.GetSubject(null);

            Assert.IsEmpty(subject.Events);
            Assert.IsEmpty(subject.GetUncommittedEvents());
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

            public ConcreteEntity(Action<Command> func) : base(new List<Event>())
            {
                this.HandleFunc = func;
            }

            private void Handle(ConcreteCommand command)
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