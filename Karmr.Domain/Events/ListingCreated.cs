namespace Karmr.Domain.Events
{
    using System;

    internal class ListingCreated : Event
    {
        internal string Name { get; }

        internal string Description { get; }

        public ListingCreated(Guid entityKey, Guid userId, string name, string description, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}