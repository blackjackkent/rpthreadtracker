using System.Security.Claims;
using System.Web.Mvc;

namespace TumblrThreadTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Maintenance()
        {
            return View();
        }
    }
}