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

    using Karmr.DomainUnitTests.Helpers;

    public class EntityTests
    {
        private readonly IClock clock = new StaticClock(DateTime.UtcNow);

        [Test]
        public void NewEntityHasEmptyEventList()
        {
            var subject = this.GetSubject(new List<IEvent>());

            Assert.IsEmpty(subject.Events);
            Assert.IsEmpty(subject.GetUncommittedEvents());
        }

        [Test]
        public void EntityConstructorAppliesExistingEvents()
        {
            var events = new List<IEvent> { new ConcreteEvent(), new ConcreteEvent() };
            var subject = this.GetSubject(events);

            Assert.AreEqual(2, subject.AppliedEventCount);
            Assert.AreEqual(2, subject.Events.Count);
            Assert.IsEmpty(subject.GetUncommittedEvents());
        }

        [Test]
        public void ExceptionThrownWhenEntityCantApplyEvent()
        {
            var events = new List<IEvent> { new UnhandledEvent() };
            Assert.Throws<UnhandledEventException>(() => this.GetSubject(events));
        }

        [Test]
        public void UncommittedEventsUpdatedWhenCommandRaisesEvent()
        {
            var events = new List<IEvent> { new ConcreteEvent(), new ConcreteEvent() };

            var subject = this.GetSubject(events);
            subject.Handle(new ConcreteCommand());

            Assert.AreEqual(3, subject.Events.Count);
            Assert.AreEqual(1, subject.GetUncommittedEvents().Count);
        }

        [Test]
        public void ExceptionThrownWhenEntityCantHandleCommand()
        {
            var subject = this.GetSubject(new List<IEvent>());
            var command = new UnhandledCommand();

            Assert.Throws<UnhandledCommandException>(() => subject.Handle(command));
        }

        [Test]
        public void ExceptionIsBubbledUpWhenHandlingCommandThrows()
        {
            Action<ConcreteEntity, Command> handleFunction = (entity, command) => throw new Exception();
            var subject = this.GetSubject(new List<IEvent>(), handleFunction);
            Assert.Throws<Exception>(() => subject.Handle(new ConcreteCommand()));
        }

        private ConcreteEntity GetSubject(IEnumerable<IEvent> events, Action<ConcreteEntity, Command> handleFunction = null)
        {
            return new ConcreteEntity(this.clock, events, handleFunction);
        }

        private class ConcreteEntity : Entity
        {
            private Action<ConcreteEntity, Command> HandleFunc { get; }

            internal int AppliedEventCount;

            public ConcreteEntity(IClock clock, IEnumerable<IEvent> events, Action<ConcreteEntity, Command> handleFunction) : base(clock, events)
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
            public ConcreteEvent() : base(Guid.Empty, Guid.Empty, DateTime.UtcNow) { }
        }

        private class UnhandledCommand : Command
        {
            internal UnhandledCommand() : base(Guid.Empty) { }
        }

        private class UnhandledEvent : Event {
            public UnhandledEvent() : base(Guid.Empty, Guid.Empty, DateTime.UtcNow) { }
        }
    }
}