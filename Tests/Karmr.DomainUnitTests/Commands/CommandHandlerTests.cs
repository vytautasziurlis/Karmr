namespace Karmr.DomainUnitTests.Commands
{
    using Karmr.Common.Contracts;
    using Karmr.Domain.Commands;
    using Karmr.Domain.Denormalizers;
    using Karmr.Domain.Entities;
    using Karmr.Domain.Events;
    using Karmr.Common.Infrastructure;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    using Karmr.DomainUnitTests.Helpers;

    public class CommandHandlerTests
    {
        private readonly IClock clock = new StaticClock(DateTime.UtcNow);

        private Mock<IEventRepository> mockRepo;
        private Mock<IDenormalizerRepository> mockDenormalizerRepo;

        [SetUp]
        public void Setup()
        {
            this.mockRepo = new Mock<IEventRepository>();
            this.mockDenormalizerRepo = new Mock<IDenormalizerRepository>();
        }

        [Test]
        public void CommandHandlerFailsWhenHandlerAintThere()
        {
            var subject = this.GetSubject(new List<Type>());
            Assert.Throws<UnhandledCommandException>(() => subject.Handle(Mock.Of<ICommand>()));
        }

        [Test]
        public void CommandHandlerFailsWhenCommandIsHandledByMultipleEntities()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Entity1), typeof(Entity2) });
            Assert.Throws<UnhandledCommandException>(() => subject.Handle(new DummyCommand1()));
        }

        [Test]
        public void CommandHandlerTriesToLoadEntityEvents()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Entity1) });

            var command = new DummyCommand1();
            subject.Handle(command);

            this.mockRepo.Verify(x => x.Get(command.EntityKey), Times.Once);
        }

        [Test]
        public void CommandHandlerPersistsEventsToRepository()
        {
            var subject = this.GetSubject(new List<Type> { typeof(Entity1) });

            var command = new DummyCommand1();
            subject.Handle(command);

            this.mockRepo.Verify(x => x.Save(typeof(Entity1), command.EntityKey, It.IsAny<DummyEvent1>(), 0), Times.Once);
        }

        private CommandHandler GetSubject(IEnumerable<Type> entityTypes)
        {
            return new CommandHandler(this.clock, this.mockRepo.Object, this.mockDenormalizerRepo.Object, entityTypes, new List<Type> { typeof(DummyDenormalizer1) });
        }

        private class DummyCommand1 : Command
        {
            internal DummyCommand1() : base(Guid.Empty)
            {
            }
        }

        private class Entity1 : Entity
        {
            private Entity1(IClock clock, IEnumerable<IEvent> events) : base(clock, events) { }

            private void Handle(DummyCommand1 command)
            {
                this.Raise(new DummyEvent1());
            }

            private void Apply(DummyEvent1 @event) { }
        }

        private class DummyEvent1 : Event
        {
            internal DummyEvent1() : base(Guid.Empty, Guid.Empty, DateTime.UtcNow)
            {
            }
        }

        private class Entity2 : Entity
        {
            private Entity2(IClock clock, IEnumerable<IEvent> events) : base(clock, events) { }

            private void Handle(DummyCommand1 command) { }
        }

        private class DummyDenormalizer1 : Denormalizer
        {
            private DummyDenormalizer1(IDenormalizerRepository repository) : base(repository) { }

            private void Apply(DummyEvent1 @event) { }
        }
    }
}