using System.Web.Mvc;

namespace GpsApp_2._5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "I'm going to describe it soon...";
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}