namespace Karmr.Domain.Events
{
    using System;

    internal class ListingUpdated : Event
    {
        internal string Description { get; }

        public ListingUpdated(Guid entityKey, Guid userId, string description, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.Description = description;
        }
    }
}