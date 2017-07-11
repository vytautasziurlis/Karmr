namespace Karmr.IntegrationTests.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Karmr.Contracts;
    using Karmr.Domain.Entities;

    using NUnit.Framework;
    using Karmr.Domain.Events;
    using Karmr.Persistence;

    public class SqlEventRepositoryTests
    {
        [Test]
        public void SavingEventAddsRowToEventsTable()
        {
            var entityType = typeof(TestEntity);
            var entityKey = Guid.NewGuid();
            var @event = new TestEvent(entityKey, Guid.NewGuid(), "description", DateTime.UtcNow);
            var subject = new SqlEventRepository(ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString);

            subject.Save(entityType, entityKey, @event, 0);

            var savedEvent = subject.Get(@event.EntityKey).First() as TestEvent;
            Assert.NotNull(savedEvent);
            Assert.AreEqual(@event.EntityKey, savedEvent.EntityKey);
            Assert.AreEqual(@event.UserId, savedEvent.UserId);
            Assert.AreEqual(@event.Description, savedEvent.Description);
            Assert.AreEqual(@event.Timestamp, savedEvent.Timestamp);
        }

        [Test]
        public void SavingSameEventTwiceThrowsException()
        {
            var entityType = typeof(TestEntity);
            var entityKey = Guid.NewGuid();
            var @event = new TestEvent(entityKey, Guid.NewGuid(), "description", DateTime.UtcNow);
            var subject = new SqlEventRepository(ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString);
            subject.Save(entityType, entityKey, @event, 0);

            Assert.Throws<System.Data.SqlClient.SqlException>(() => subject.Save(entityType, entityKey, @event, 0));
        }

        internal class TestEntity : Entity
        {
            public TestEntity(IClock clock, IEnumerable<IEvent> events) : base(clock, events)
            {
            }
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