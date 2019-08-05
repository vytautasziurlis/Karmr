using System;

namespace Karmr.Domain.Events
{
    internal class ListingArchived : Event
    {
        public ListingArchived(Guid entityKey, Guid userId, DateTime timestamp) : base(entityKey, userId, timestamp) { }
    }
}