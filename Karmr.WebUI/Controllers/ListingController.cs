using System.Linq;
using System.Web.Mvc;
using Karmr.Common.Contracts;
using Karmr.Domain.Commands;
using Karmr.Domain.Queries;
using Karmr.WebUI.Models.Listing;
using Microsoft.AspNet.Identity;

namespace Karmr.WebUI.Controllers
{
    [Authorize]
    public class ListingController : Controller
    {
        private readonly ListingQueries listingQueries;
        private readonly ICommandHandler commandHandler;

        public ListingController(ListingQueries listingQueries, ICommandHandler commandHandler)
        {
            this.listingQueries = listingQueries;
            this.commandHandler = commandHandler;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search(ListingSearchFormModel formModel)
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var x = userId.Length;
            }

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

        [HttpGet]
        public ActionResult Manage()
        {
            var listings = this.listingQueries
                .GetByUserId(Helpers.UserId(this.User))
                .Select(x => new ListingViewModel(x));
            return View(listings);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateListingFormModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Helpers.UserId(this.User);
                var geoLocation = Helpers.GeoLocation(model.Latitude, model.Longitude); 
                var command = new CreateListingCommand(userId, model.Name, model.Description, geoLocation);
                this.commandHandler.Handle(command);
            }
            return RedirectToAction("Manage");
        }
    }
}