using System;

namespace Karmr.Domain.Events
{
    internal class ListingOfferAccepted : Event
    {
        internal Guid OfferId { get; }

        public ListingOfferAccepted(Guid entityKey, Guid userId, Guid offerId, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.OfferId = offerId;
        }
    }
}