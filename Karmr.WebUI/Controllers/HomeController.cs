using System.Web.Mvc;

using Karmr.Domain.Queries;
using Karmr.Persistence;

namespace Karmr.WebUI.Controllers
{
    public class HomeController : Controller
    {
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
            var query = new ListingQueries(new QueryRepository("Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;"));
            var listings = query.GetAll();
            return View();
        }
    }
}