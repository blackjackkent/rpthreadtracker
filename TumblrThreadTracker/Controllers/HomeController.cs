namespace TumblrThreadTracker.Controllers
{
	using System.Web.Mvc;

	/// <summary>
	/// Controller class for base MVC views
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// MVC view that contains all front-end scripts
		/// </summary>
		/// <returns>ActionResult display class</returns>
		public ActionResult Index()
		{
			return View();
		}
	}
}