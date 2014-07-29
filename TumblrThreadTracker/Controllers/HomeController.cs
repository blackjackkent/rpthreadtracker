using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;
using TumblrThreadTracker.Services;
using WebMatrix.WebData;

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