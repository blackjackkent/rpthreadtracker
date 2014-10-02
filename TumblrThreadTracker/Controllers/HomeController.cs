using System.Web.Mvc;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;

namespace TumblrThreadTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserBlogRepository _blogRepository;
        private readonly IUserThreadRepository _threadRepository;

        public HomeController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}