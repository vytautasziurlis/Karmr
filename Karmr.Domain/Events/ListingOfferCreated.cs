using System;

namespace Karmr.Domain.Events
{
    internal class ListingOfferCreated : Event
    {
        internal Guid OfferId { get; }

        public ListingOfferCreated(Guid entityKey, Guid userId, Guid offerId, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.OfferId = offerId;
        }
    }
}