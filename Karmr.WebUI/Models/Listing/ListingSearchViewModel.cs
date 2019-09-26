using System.Collections.Generic;

namespace Karmr.WebUI.Models.Listing
{
    public sealed class ListingSearchViewModel
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Address { get; set; }

        public IEnumerable<ListingViewModel> Listings { get; set; }
    }
}