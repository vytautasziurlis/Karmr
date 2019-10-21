using System;
using System.Collections.Generic;

namespace Karmr.Domain.Queries.Models
{
    public sealed class ListingDetails
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public string Name { get; }

        public string Description { get; }

        public string LocationName { get; }

        public decimal Latitude { get; }

        public decimal Longitude { get; }

        public bool IsArchived { get; }

        public DateTime Created { get; }

        public DateTime? Updated { get; }

        internal IEnumerable<DiscussionThread> DiscussionThreads { get; set; }
    }
}