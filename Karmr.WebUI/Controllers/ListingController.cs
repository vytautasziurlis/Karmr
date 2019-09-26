using System.Web.Mvc;
using Karmr.WebUI.Models.Listing;

namespace Karmr.WebUI.Controllers
{
    public class ListingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(ListingSearchFormModel formModel)
        {
            var model = new ListingSearchViewModel
            {
                Latitude = formModel.Latitude.GetValueOrDefault(),
                Longitude = formModel.Longitude.GetValueOrDefault(),
                Address = formModel.Address
            };

            return View(model);
        }
    }
}