namespace Karmr.DomainUnitTests.Events
{
    using Karmr.Domain.Events;
    using Karmr.DomainUnitTests.Helpers;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    public class EventTests
    {
        [Test]
        public void AllEventDescendantsAreImmutable()
        {
            Assembly.GetAssembly(typeof(Event)).GetTypes().Where(t => t.IsSubclassOf(typeof(Event))).ToList()
                .ForEach(TypeAssert.IsImmutable);
        }

        [Test]
        public void AllEventDescendantsHaveExactlyOneConstructor()
        {
            Assembly.GetAssembly(typeof(Event)).GetTypes().Where(t => t.IsSubclassOf(typeof(Event))).ToList()
                .ForEach(this.AssertEventHasExactlyOneConstructor);
        }

        private void AssertEventHasExactlyOneConstructor(Type eventType)
        {
            var bindingFlags = BindingFlags.Public
                               | BindingFlags.Static
                               | BindingFlags.NonPublic
                               | BindingFlags.Instance;

            var constructorCount = eventType.GetConstructors(bindingFlags).Length;
            Assert.AreEqual(1, constructorCount, "Event {0} has {1} constructors, expected 1", eventType, constructorCount);
        }
    }
}