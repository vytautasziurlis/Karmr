using System;
using System.Collections.Generic;
using System.Linq;

using Karmr.Common.Contracts;
using Karmr.Domain.Commands;
using Karmr.Domain.Events;
using Karmr.Common.Types;

namespace Karmr.Domain.Entities
{
    internal class Listing : Entity
    {
        internal Guid Id { get; private set; }

        internal Guid UserId { get; private set; }

        internal string Name { get; private set; }

        internal string Description { get; private set; }

        internal GeoLocation? Location { get; private set; }

        internal bool IsPublic { get; private set; }

        internal bool IsArchived { get; private set; }

        internal List<DiscussionThread> DiscussionThreads { get; private set; }

        internal List<Offer> Offers { get; private set; }

        internal Listing(IClock clock, IEnumerable<IEvent> events) : base(clock, events) { }

        private void Handle(CreateListingCommand command)
        {
            if (this.Events.Any())
            {
                throw new Exception(string.Format("Expected empty list of events, found {0} events", this.Events.Count));
            }
            this.Raise(new ListingCreated(command.EntityKey, command.UserId, command.Name, command.Description, command.Location, this.Clock.UtcNow));
        }

        private void Handle(UpdateListingCommand command)
        {
            if (!this.Events.Any(x => x is ListingCreated))
            {
                throw new Exception(string.Format("ListingCreated event missing (found {0} events)", this.Events.Count));
            }
            if (this.UserId != command.UserId)
            {
                throw new Exception("Permission denied");
            }
            this.Raise(new ListingUpdated(command.EntityKey, command.UserId, command.Name, command.Description, command.Location, this.Clock.UtcNow));
        }

        private void Handle(ArchiveListingCommand command)
        {
            if (!this.Events.Any(x => x is ListingCreated))
            {
                throw new Exception(string.Format("ListingCreated event missing (found {0} events)", this.Events.Count));
            }
            if (this.UserId != command.UserId)
            {
                throw new Exception("Permission denied");
            }
            this.Raise(new ListingArchived(command.EntityKey, command.UserId, this.Clock.UtcNow));
        }

        private void Handle(CreateListingDiscussionThreadCommand command)
        {
            if (!this.Events.Any(x => x is ListingCreated))
            {
                throw new Exception(string.Format("ListingCreated event missing (found {0} events)", this.Events.Count));
            }
            if (this.UserId == command.UserId)
            {
                throw new Exception("Permission denied");
            }
            if (this.DiscussionThreads.Any(x => x.UserId == command.UserId))
            {
                throw new Exception("Discussion thread already exists");
            }

            var threadId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            this.Raise(new ListingDiscussionThreadCreated(command.EntityKey, command.UserId, threadId, this.Clock.UtcNow));
            this.Raise(new ListingDiscussionPostCreated(command.EntityKey, command.UserId, postId, threadId, command.Content, this.Clock.UtcNow));
        }

        private void Handle(CreateListingOfferCommand command)
        {
            if (!this.Events.Any(x => x is ListingCreated))
            {
                throw new Exception(string.Format("ListingCreated event missing (found {0} events)", this.Events.Count));
            }
            if (this.UserId == command.UserId)
            {
                throw new Exception("Permission denied");
            }
            if (this.Offers.Any(x => x.UserId == command.UserId))
            {
                throw new Exception("Offer already exists");
            }

            this.Raise(new ListingOfferCreated(command.EntityKey, command.UserId, this.Clock.UtcNow));
        }

        private void Apply(ListingCreated @event)
        {
            this.Id = @event.EntityKey;
            this.UserId = @event.UserId;
            this.Name = @event.Name;
            this.Description = @event.Description;
            this.Location = @event.Location;
            this.IsPublic = true;
            this.IsArchived = false;
            this.DiscussionThreads = new List<DiscussionThread>();
            this.Offers = new List<Offer>();
        }

        private void Apply(ListingUpdated @event)
        {
            this.Name = @event.Name;
            this.Description = @event.Description;
            this.Location = @event.Location;
        }

        private void Apply(ListingArchived @event)
        {
            this.IsArchived = true;
        }

        private void Apply(ListingDiscussionThreadCreated @event)
        {
            this.DiscussionThreads.Add(new DiscussionThread(@event.ThreadId, @event.UserId));
        }

        private void Apply(ListingDiscussionPostCreated @event)
        {
            var newPost = new DiscussionPost(@event.UserId, @event.Content);
            this.DiscussionThreads.Single(x => x.ThreadId == @event.ThreadId).Posts.Add(newPost);
        }

        private void Apply(ListingOfferCreated @event)
        {
            this.Offers.Add(new Offer(@event.UserId, false));
        }
    }
}