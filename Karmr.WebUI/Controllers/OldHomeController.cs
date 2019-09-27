using System.Web.Mvc;
using Karmr.Common.Contracts;
using Karmr.Domain.Queries;

namespace Karmr.WebUI.Controllers
{
    public class OldHomeController : Controller
    {
        private readonly ICommandHandler commandHandler;
        private readonly ListingQueries listingQueries;

        public OldHomeController(ICommandHandler commandHandler, ListingQueries listingQueries)
        {
            this.commandHandler = commandHandler;
            this.listingQueries = listingQueries;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestPage()
        {
            var listings = this.listingQueries.GetAll();
            return View();
        }
    }
}