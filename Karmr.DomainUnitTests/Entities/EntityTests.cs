namespace Karmr.DomainUnitTests.Entities
{
    using Karmr.Contracts;
    using Karmr.Domain.Commands;
    using Karmr.Domain.Entities;
    using Karmr.Domain.Events;
    using Karmr.Domain.Infrastructure;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public class EntityTests
    {
        [Test]
        public void NewEntityHasEmptyEventList()
        {
            var subject = new ConcreteEntity(new List<IEvent>());

            Assert.IsEmpty(subject.Events);
            Assert.IsEmpty(subject.GetUncommittedEvents());
        }

        [Test]
        public void EntityConstructorAppliesExistingEvents()
        {
            var events = new List<IEvent> { new ConcreteEvent(), new ConcreteEvent() };
            var subject = new ConcreteEntity(events);

            Assert.AreEqual(2, subject.AppliedEventCount);
            Assert.AreEqual(2, subject.Events.Count);
            Assert.IsEmpty(subject.GetUncommittedEvents());
        }

        [Test]
        public void ExceptionThrownWhenEntityCantApplyEvent()
        {
            var events = new List<IEvent> { new UnhandledEvent() };
            Assert.Throws<UnhandledEventException>(() => new ConcreteEntity(events));
        }

        [Test]
        public void UncommittedEventsUpdatedWhenCommandRaisesEvent()
        {
            var events = new List<IEvent> { new ConcreteEvent(), new ConcreteEvent() };

            var subject = new ConcreteEntity(events, null);
            subject.Handle(new ConcreteCommand());

            Assert.AreEqual(3, subject.Events.Count);
            Assert.AreEqual(1, subject.GetUncommittedEvents().Count);
        }

        [Test]
        public void ExceptionThrownWhenEntityCantHandleCommand()
        {
            var subject = new ConcreteEntity(new List<IEvent>());
            var command = new UnhandledCommand();

            Assert.Throws<UnhandledCommandException>(() => subject.Handle(command));
        }

        [Test]
        public void ExceptionIsBubbledUpWhenHandlingCommandThrows()
        {
            Action<ConcreteEntity, Command> handleFunction = (entity, command) => throw new Exception();
            var subject = new ConcreteEntity(new List<IEvent>(), handleFunction);
            Assert.Throws<Exception>(() => subject.Handle(new ConcreteCommand()));
        }

        private class ConcreteEntity : Entity
        {
            private Action<ConcreteEntity, Command> HandleFunc { get; }

            internal int AppliedEventCount;

            public ConcreteEntity(IEnumerable<IEvent> events) : base(events)
            {
                this.HandleFunc = null;
            }

            public ConcreteEntity(IEnumerable<IEvent> events, Action<ConcreteEntity, Command> handleFunction) : base(events)
            {
                this.HandleFunc = handleFunction;
            }

            private void Handle(ConcreteCommand command)
            {
                this.HandleFunc?.Invoke(this, command);
                this.Raise(new ConcreteEvent());
            }

            private void Apply(ConcreteEvent @event)
            {
                this.AppliedEventCount++;
            }
        }

        private class ConcreteCommand : Command
        {
            internal ConcreteCommand() : base(Guid.Empty) { }
        }

        private class ConcreteEvent : Event
        {
            public ConcreteEvent() : base(Guid.Empty, Guid.Empty) { }
        }

        private class UnhandledCommand : Command
        {
            internal UnhandledCommand() : base(Guid.Empty) { }
        }

        private class UnhandledEvent : Event {
            public UnhandledEvent() : base(Guid.Empty, Guid.Empty) { }
        }
    }
}