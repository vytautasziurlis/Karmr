using System;

namespace Karmr.Domain.Events
{
    internal class ListingOfferCreated : Event
    {
        public ListingOfferCreated(Guid entityKey, Guid userId, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
        }
    }
}