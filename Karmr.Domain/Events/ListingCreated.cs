namespace Karmr.Domain.Events
{
    using System;

    internal class ListingCreated : Event
    {
        internal string Description { get; }

        public ListingCreated(Guid entityKey, Guid userId, string description, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.Description = description;
        }
    }
}