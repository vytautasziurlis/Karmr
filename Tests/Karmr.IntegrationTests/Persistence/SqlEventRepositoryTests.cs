namespace Karmr.IntegrationTests.Persistence
{
    using Karmr.Contracts;
    using Karmr.Domain.Entities;
    using Karmr.Domain.Events;
    using Karmr.Persistence;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public class SqlEventRepositoryTests
    {
        private Type entityType;
        private Guid entityKey;
        private TestEvent @event;
        private SqlEventRepository subject;

        [SetUp]
        public void Setup()
        {
            this.entityType = typeof(TestEntity);
            this.entityKey = Guid.NewGuid();
            this.@event = new TestEvent(this.entityKey, Guid.NewGuid(), "description", DateTime.UtcNow);
            this.subject = new SqlEventRepository(ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString);
        }

        [Test]
        public void SavingEventWithNegativeSequenceThrowsException()
        {
            Assert.Throws<Exception>(() => this.subject.Save(this.entityType, this.entityKey, this.@event, -1));
            Assert.IsEmpty(this.subject.Get(this.@event.EntityKey));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void SavingFirstEventEventWithNonZeroSequenceThrowsException(int sequenceNumber)
        {
            Assert.Throws<SqlException>(() => this.subject.Save(this.entityType, this.entityKey, this.@event, sequenceNumber));
            Assert.IsEmpty(this.subject.Get(this.@event.EntityKey));
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(3)]
        public void SavingOutOfSequenceEventThrowsException(int sequenceNumber)
        {
            this.subject.Save(this.entityType, this.entityKey, this.@event, 0);

            Assert.Throws<SqlException>(() => this.subject.Save(this.entityType, this.entityKey, this.@event, sequenceNumber));
            Assert.AreEqual(1, this.subject.Get(this.@event.EntityKey).Count());
        }

        [Test]
        public void SavingFirstEventAddsRowToEventsTable()
        {
            this.subject.Save(this.entityType, this.entityKey, this.@event, 0);

            var savedEvent = this.subject.Get(this.@event.EntityKey).Single() as TestEvent;

            Assert.NotNull(savedEvent);
            Assert.AreEqual(this.@event.EntityKey, savedEvent.EntityKey);
            Assert.AreEqual(this.@event.UserId, savedEvent.UserId);
            Assert.AreEqual(this.@event.Description, savedEvent.Description);
            Assert.AreEqual(this.@event.Timestamp, savedEvent.Timestamp);
        }

        [Test]
        public void SavingSecondEventAddsRowToEventsTable()
        {
            var firstEvent = new TestEvent(this.entityKey, Guid.NewGuid(), "first event", DateTime.UtcNow);
            this.subject.Save(this.entityType, this.entityKey, firstEvent, 0);
            this.subject.Save(this.entityType, this.entityKey, this.@event, 1);

            var savedEvents = this.subject.Get(this.@event.EntityKey).ToList();
            Assert.AreEqual(2, savedEvents.Count());

            var savedEvent = savedEvents.Last() as TestEvent;
            Assert.NotNull(savedEvent);
            Assert.AreEqual(this.@event.EntityKey, savedEvent.EntityKey);
            Assert.AreEqual(this.@event.UserId, savedEvent.UserId);
            Assert.AreEqual(this.@event.Description, savedEvent.Description);
            Assert.AreEqual(this.@event.Timestamp, savedEvent.Timestamp);
        }

        [Test]
        public void SavingSameEventTwiceThrowsException()
        {
            this.subject.Save(this.entityType, this.entityKey, this.@event, 0);

            Assert.Throws<SqlException>(() => this.subject.Save(this.entityType, this.entityKey, this.@event, 0));
            Assert.AreEqual(1, this.subject.Get(this.@event.EntityKey).Count());
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