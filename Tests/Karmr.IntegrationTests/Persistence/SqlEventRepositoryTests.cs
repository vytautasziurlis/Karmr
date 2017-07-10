namespace Karmr.IntegrationTests.Persistence
{
    using System;

    using NUnit.Framework;
    using Karmr.Domain.Events;
    using Karmr.Persistence;

    public class SqlEventRepositoryTests
    {
        [Test]
        public void SavingEventAddsRowToEventsTable()
        {
            var @event = new TestEvent(Guid.NewGuid(), Guid.NewGuid(), "description", DateTime.UtcNow);
            var subject = new SqlEventRepository();

            subject.Save(@event, typeof(TestEvent), 0);

            var events = subject.Get(typeof(TestEvent), @event.EntityKey);
        }

        internal class TestEvent : Event
        {
            internal string Description { get; }

            public TestEvent(Guid entityKey, Guid userId, string description, DateTime timestamp) : base(entityKey, userId, timestamp)
            {
                this.Description = description;
            }
        }
    }
}