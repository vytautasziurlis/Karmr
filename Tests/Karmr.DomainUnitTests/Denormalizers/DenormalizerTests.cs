using Karmr.Common.Contracts;
using Karmr.Common.Infrastructure;
using Karmr.Domain.Denormalizers;
using Moq;
using NUnit.Framework;

namespace Karmr.DomainUnitTests.Denormalizers
{
    [TestFixture]
    public class DenormalizerTests
    {
        [Test]
        public void ApplyingEventCallsConcreteMethod()
        {
            var subject = new DummyDenormalizer(Mock.Of<IDenormalizerRepository>());

            var @event = new DummyEvent1();
            subject.Apply(@event);

            Assert.AreSame(@event, subject.lastAppliedEvent);
        }

        [Test]
        public void DenormalizerThrowsWhenApplyingUnhandledEvent()
        {
            var subject = new DummyDenormalizer(Mock.Of<IDenormalizerRepository>());

            var @event = new DummyEvent2();

            Assert.Throws<UnhandledEventException>(() => subject.Apply(@event));
        }

        private class DummyDenormalizer : Denormalizer
        {
            public IEvent lastAppliedEvent;

            public DummyDenormalizer(IDenormalizerRepository repository) : base(repository) { }

            private void Apply(DummyEvent1 @event)
            {
                this.lastAppliedEvent = @event;
            }
        }

        private class DummyEvent1 : IEvent { }

        private class DummyEvent2 : IEvent { }
    }
}