using System;

namespace Karmr.Domain.Queries.Models
{
    public sealed class Listing
    {
        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime Created { get; }

        public DateTime? Updated { get; }
    }
}