using System;
using Karmr.Common.Types;

namespace Karmr.Domain.Events
{
    internal class ListingUpdated : Event
    {
        internal string Name { get; }

        internal string Description { get; }

        internal GeoLocation? Location { get; }

        public ListingUpdated(Guid entityKey, Guid userId, string name, string description, GeoLocation? location, DateTime timestamp) : base(entityKey, userId, timestamp)
        {
            this.Name = name;
            this.Description = description;
            this.Location = location;
        }
    }
}