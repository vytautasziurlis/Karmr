using System;

namespace Karmr.WebUI.Models.Listing
{
    public sealed class ListingViewModel
    {
        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime Created { get; }

        public DateTime? Updated { get; }

        public ListingViewModel(Domain.Queries.Models.Listing listing)
        {
            Id = listing.Id;
            Name = listing.Name;
            Description = listing.Description;
            Created = listing.Created;
            Updated = listing.Updated;
        }
    }
}