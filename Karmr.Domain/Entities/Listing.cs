namespace Karmr.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Karmr.Domain.Commands;
    using Karmr.Domain.Events;

    internal class Listing : Entity
    {
        internal Guid Id { get; private set; }

        internal Guid UserId { get; private set; }

        internal string Description { get; private set; }

        internal Listing(IEnumerable<Event> events) : base(events) { }

        private void Handle(CreateListingCommand command)
        {
            if (this.Events.Any())
            {
                throw new Exception(string.Format("Expected empty list of events, found {0} events", this.Events.Count));
            }
            this.Raise(new ListingCreated(command.EntityKey, command.UserId, command.Description));
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
            this.Raise(new ListingUpdated(command.EntityKey, command.UserId, command.Description));
        }

        private void Apply(ListingCreated @event)
        {
            this.Id = @event.EntityKey;
            this.UserId = @event.UserId;
            this.Description = @event.Description;
        }

        private void Apply(ListingUpdated @event)
        {
            this.Description = @event.Description;
        }
    }
}