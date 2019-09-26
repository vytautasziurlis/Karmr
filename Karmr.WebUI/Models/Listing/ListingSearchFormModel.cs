using System.ComponentModel.DataAnnotations;

namespace Karmr.WebUI.Models.Listing
{
    public class ListingSearchFormModel
    {
        [Required(ErrorMessage = "Please enter your location to continue")]
        public decimal? Latitude { get; set; }

        [Required(ErrorMessage = "Please enter your location to continue")]
        public decimal? Longitude { get; set; }

        public string Address { get; set; }
    }
}