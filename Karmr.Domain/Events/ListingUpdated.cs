namespace Karmr.Domain.Events
{
    using System;

    internal class ListingUpdated : Event
    {
        internal string Description { get; }

        public ListingUpdated(Guid entityKey, Guid userId, string description) : base(entityKey, userId)
        {
            this.Description = description;
        }
    }
}