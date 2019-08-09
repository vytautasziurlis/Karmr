namespace Karmr.DomainUnitTests.Entities
{
    using Builders;
    using Karmr.Common.Contracts;
    using Karmr.Common.Types;
    using Karmr.Domain.Commands;
    using Karmr.Domain.Entities;
    using Karmr.Domain.Events;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Karmr.DomainUnitTests.Helpers;

    public class ListingTests
    {
        private readonly IClock clock = new StaticClock(DateTime.UtcNow);

        [Test]
        public void HandlingCreateListingCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            Assert.AreEqual(command.EntityKey, subject.Id);
            Assert.AreEqual(command.UserId, subject.UserId);
            Assert.AreEqual(command.Name, subject.Name);
            Assert.AreEqual(command.Description, subject.Description);
            Assert.AreEqual(command.Location, subject.Location);
            Assert.AreEqual(true, subject.IsPublic);
            Assert.AreEqual(false, subject.IsArchived);
            Assert.IsEmpty(subject.DiscussionThreads);
            Assert.IsEmpty(subject.Offers);
        }

        [Test]
        public void HandlingCreateListingCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(1, subject.Events.Count);
            Assert.AreEqual(1, uncommittedEvents.Count);
            var @event = uncommittedEvents.First() as ListingCreated;
            Assert.NotNull(@event);
            Assert.AreEqual(command.EntityKey, @event.EntityKey);
            Assert.AreEqual(command.UserId, @event.UserId);
            Assert.AreEqual(command.Name, @event.Name);
            Assert.AreEqual(command.Description, @event.Description);
            Assert.AreEqual(command.Location, @event.Location);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        [Test]
        public void HandlingCreateListingCommandTwiceThrowsException()
        {
            var subject = this.GetSubject();

            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingUpdateListingCommandRequiresCreateEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<UpdateListingCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingUpdateListingCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>().With(x => x.UserId, Guid.NewGuid()).Build();

            Assert.Throws<Exception>(() => subject.Handle(updateCommand));
        }

        [Test]
        public void HandlingUpdateListingCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.Name, createCommand.Name + " tail")
                .With(x => x.Description, createCommand.Description + " tail")
                .With(x => x.Location, new GeoLocation(
                    (createCommand.Location?.Latitude ?? 0) + 0.01m,
                    (createCommand.Location?.Longitude ?? 0) + 0.02m))
                .Build();

            subject.Handle(updateCommand);

            Assert.AreEqual(updateCommand.Name, subject.Name);
            Assert.AreEqual(updateCommand.Description, subject.Description);
            Assert.AreEqual(updateCommand.Location, subject.Location);
        }

        [Test]
        public void HandlingUpdateListingCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.Name, createCommand.Name + " tail")
                .With(x => x.Description, createCommand.Description + " tail")
                .With(x => x.Location, new GeoLocation(
                    (createCommand.Location?.Latitude ?? 0) + 0.01m,
                    (createCommand.Location?.Longitude ?? 0) + 0.02m))
                .Build();

            subject.Handle(updateCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(2, subject.Events.Count);
            Assert.AreEqual(2, uncommittedEvents.Count);
            var @event = uncommittedEvents.Last() as ListingUpdated;
            Assert.NotNull(@event);
            Assert.AreEqual(updateCommand.EntityKey, @event.EntityKey);
            Assert.AreEqual(updateCommand.UserId, @event.UserId);
            Assert.AreEqual(updateCommand.Name, @event.Name);
            Assert.AreEqual(updateCommand.Description, @event.Description);
            Assert.AreEqual(updateCommand.Location, @event.Location);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        [Test]
        public void HandlingArchiveListingCommandRequiresCreateEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<ArchiveListingCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingArchiveListingCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var archiveCommand = new CommandBuilder<ArchiveListingCommand>().With(x => x.UserId, Guid.NewGuid()).Build();

            Assert.Throws<Exception>(() => subject.Handle(archiveCommand));
        }

        [Test]
        public void HandlingArchiveListingCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var archiveCommand = new CommandBuilder<ArchiveListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .Build();

            subject.Handle(archiveCommand);

            Assert.AreEqual(true, subject.IsArchived);
        }

        [Test]
        public void HandlingArchiveListingCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var archiveCommand = new CommandBuilder<ArchiveListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .Build();

            subject.Handle(archiveCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(2, subject.Events.Count);
            Assert.AreEqual(2, uncommittedEvents.Count);
            var @event = uncommittedEvents.Last() as ListingArchived;
            Assert.NotNull(@event);
            Assert.AreEqual(archiveCommand.EntityKey, @event.EntityKey);
            Assert.AreEqual(archiveCommand.UserId, @event.UserId);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        [Test]
        public void HandlingCreateListingDiscussionThreadCommandRequiresCreateEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingDiscussionThreadCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingCreateListingDiscussionThreadCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createThreadCommand = new CommandBuilder<CreateListingDiscussionThreadCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .Build();

            Assert.Throws<Exception>(() => subject.Handle(createThreadCommand));
        }

        [Test]
        public void HandlingCreateListingDiscussionThreadCommandTwiceThrows()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createThreadCommand = new CommandBuilder<CreateListingDiscussionThreadCommand>().Build();
            subject.Handle(createThreadCommand);

            Assert.Throws<Exception>(() => subject.Handle(createThreadCommand));
        }

        [Test]
        public void HandlingCreateListingDiscussionThreadCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createThreadCommand = new CommandBuilder<CreateListingDiscussionThreadCommand>().Build();

            subject.Handle(createThreadCommand);

            Assert.AreEqual(createThreadCommand.UserId, subject.DiscussionThreads.First().UserId);
        }

        [Test]
        public void HandlingCreateListingDiscussionThreadCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createThreadCommand = new CommandBuilder<CreateListingDiscussionThreadCommand>().Build();

            subject.Handle(createThreadCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(3, subject.Events.Count);
            Assert.AreEqual(3, uncommittedEvents.Count);

            var event1 = uncommittedEvents[1] as ListingDiscussionThreadCreated;
            Assert.NotNull(event1);
            Assert.AreEqual(createThreadCommand.EntityKey, event1.EntityKey);
            Assert.AreEqual(createThreadCommand.UserId, event1.UserId);
            Assert.AreEqual(this.clock.UtcNow, event1.Timestamp);

            var event2 = uncommittedEvents.Last() as ListingDiscussionPostCreated;
            Assert.NotNull(event2);
            Assert.AreEqual(createThreadCommand.EntityKey, event2.EntityKey);
            Assert.AreEqual(createThreadCommand.UserId, event2.UserId);
            Assert.AreEqual(event1.ThreadId, event2.ThreadId);
            Assert.AreEqual(createThreadCommand.Content, event2.Content);
            Assert.AreEqual(this.clock.UtcNow, event2.Timestamp);
        }

        [Test]
        public void HandlingCreateListingOfferCommandRequiresCreateEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingOfferCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingCreateListingOfferCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .Build();

            Assert.Throws<Exception>(() => subject.Handle(createOfferCommand));
        }

        [Test]
        public void HandlingCreateListingOfferCommandTwiceThrows()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();
            subject.Handle(createOfferCommand);

            Assert.Throws<Exception>(() => subject.Handle(createOfferCommand));
        }

        [Test]
        public void HandlingCreateListingOfferCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();

            subject.Handle(createOfferCommand);

            var offer = subject.Offers.First();
            Assert.AreEqual(createOfferCommand.UserId, offer.UserId);
            Assert.AreEqual(false, offer.Accepted);
        }

        [Test]
        public void HandlingCreateListingOfferCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();

            subject.Handle(createOfferCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(2, subject.Events.Count);
            Assert.AreEqual(2, uncommittedEvents.Count);

            var @event = uncommittedEvents[1] as ListingOfferCreated;
            Assert.NotNull(@event);
            Assert.AreEqual(createOfferCommand.EntityKey, @event.EntityKey);
            Assert.AreEqual(createOfferCommand.UserId, @event.UserId);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        [Test]
        public void HandlingAcceptListingOfferCommandRequiresCreateEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<AcceptListingOfferCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingAcceptListingOfferCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();
            subject.Handle(createOfferCommand);

            var acceptOfferCommand = new CommandBuilder<AcceptListingOfferCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(acceptOfferCommand));
        }

        [Test]
        public void HandlingAcceptListingOfferCommandChecksOfferId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();
            subject.Handle(createOfferCommand);

            var acceptOfferCommand = new CommandBuilder<AcceptListingOfferCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .Build();

            Assert.Throws<Exception>(() => subject.Handle(acceptOfferCommand));
        }

        [Test]
        public void HandlingAcceptListingOfferCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();
            subject.Handle(createOfferCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            var createEvent = uncommittedEvents.Last() as ListingOfferCreated;

            var acceptOfferCommand = new CommandBuilder<AcceptListingOfferCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.OfferId, createEvent.OfferId)
                .Build();

            subject.Handle(acceptOfferCommand);

            var offer = subject.Offers.First();
            Assert.AreEqual(acceptOfferCommand.UserId, offer.UserId);
            Assert.AreEqual(true, offer.Accepted);
        }

        [Test]
        public void HandlingAcceptListingOfferCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var createOfferCommand = new CommandBuilder<CreateListingOfferCommand>().Build();
            subject.Handle(createOfferCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            var createEvent = uncommittedEvents.Last() as ListingOfferCreated;

            var acceptOfferCommand = new CommandBuilder<AcceptListingOfferCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.OfferId, createEvent.OfferId)
                .Build();

            subject.Handle(acceptOfferCommand);

            uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(3, subject.Events.Count);
            Assert.AreEqual(3, uncommittedEvents.Count);

            var @event = uncommittedEvents.Last() as ListingOfferAccepted;
            Assert.NotNull(@event);
            Assert.AreEqual(acceptOfferCommand.EntityKey, @event.EntityKey);
            Assert.AreEqual(acceptOfferCommand.UserId, @event.UserId);
            Assert.AreEqual(acceptOfferCommand.OfferId, @event.OfferId);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        private Listing GetSubject()
        {
            return new Listing(this.clock, new List<IEvent>());
        }
    }
}