namespace Karmr.DomainUnitTests.Denormalizers
{
    using Karmr.Common.Contracts;
    using Karmr.Domain.Denormalizers;
    using Karmr.Domain.Events;
    using Karmr.Common.Infrastructure;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public class DenormalizerHandlerTests
    {
        private Mock<IDenormalizerRepository> mockDenormalizerRepo;

        [SetUp]
        public void Setup()
        {
            this.mockDenormalizerRepo = new Mock<IDenormalizerRepository>();
        }

        [Test]
        public void DenormalizerHandlerFailsWhenHandlerAintThere()
        {
            var subject = this.GetSubject(new List<Type>());
            Assert.Throws<UnhandledEventException>(() => subject.Handle(new List<IEvent> { new DummyEvent1() }));
        }

        [Test]
        public void DenormalizerHandlerFailsWhenCommandIsHandledByMultipleEntities()
        {
            var subject = this.GetSubject(new List<Type> { typeof(DummyDenormalizer1), typeof(DummyDenormalizer2) });
            Assert.Throws<UnhandledEventException>(() => subject.Handle(new List<IEvent> { new DummyEvent1() }));
        }

        private DenormalizerHandler GetSubject(IEnumerable<Type> denormalizerTypes)
        {
            return new DenormalizerHandler(this.mockDenormalizerRepo.Object, denormalizerTypes);
        }

        private class DummyEvent1 : Event
        {
            internal DummyEvent1() : base(Guid.Empty, Guid.Empty, DateTime.UtcNow)
            {
            }
        }

        private class DummyDenormalizer1 : Denormalizer
        {
            private DummyDenormalizer1(IDenormalizerRepository repository) : base(repository) { }

            private void Apply(DummyEvent1 @event) { }
        }

        private class DummyDenormalizer2 : Denormalizer
        {
            private DummyDenormalizer2(IDenormalizerRepository repository) : base(repository) { }

            private void Apply(DummyEvent1 @event) { }
        }
    }
}