using System.Linq;
using System.Web.Mvc;
using Karmr.Domain.Queries;
using Karmr.WebUI.Models.Listing;

namespace Karmr.WebUI.Controllers
{
    public class ListingController : Controller
    {
        private readonly ListingQueries listingQueries;

        public ListingController(ListingQueries listingQueries)
        {
            this.listingQueries = listingQueries;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(ListingSearchFormModel formModel)
        {
            var listings = this.listingQueries.GetAll().Select(x => new ListingViewModel(x));

            var model = new ListingSearchViewModel
            {
                Latitude = formModel.Latitude.GetValueOrDefault(),
                Longitude = formModel.Longitude.GetValueOrDefault(),
                Address = formModel.Address,
                Listings = listings
            };

            return View(model);
        }
    }
}