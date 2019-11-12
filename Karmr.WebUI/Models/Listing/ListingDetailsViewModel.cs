using System;
using System.Collections.Generic;
using System.Linq;

namespace Karmr.WebUI.Models.Listing
{
    public sealed class ListingDetailsViewModel
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public string Name { get; }

        public string Description { get; }

        public string LocationName { get; }

        public decimal Latitude { get; }

        public decimal Longitude { get; }

        public DateTime Created { get; }

        public DateTime? Updated { get; }

        public IEnumerable<DiscussionThreadViewModel> DiscussionThreads { get; }

        public ListingDetailsViewModel(Domain.Queries.Models.ListingDetails listing)
        {
            Id = listing.Id;
            UserId = listing.UserId;
            Name = listing.Name;
            Description = listing.Description;
            LocationName = listing.LocationName;
            Latitude = listing.Latitude;
            Longitude = listing.Longitude;
            Created = listing.Created;
            Updated = listing.Updated;
            DiscussionThreads = listing.DiscussionThreads.Select(x => new DiscussionThreadViewModel(x));
        }
    }
}