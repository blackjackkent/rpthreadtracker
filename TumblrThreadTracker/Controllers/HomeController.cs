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

        /*public ActionResult Threads()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult GetThreads()
        {
            IEnumerable<Blog> blogs =
                BlogFactory.BuildFromDataModel(_blogRepository.GetUserBlogs(WebSecurity.GetUserId(User.Identity.Name)));
            var manager = new ThreadManager
            {
                UserId = WebSecurity.GetUserId(User.Identity.Name),
                UserBlogs = blogs
            };
            var viewThreads = new List<Thread>();
            foreach (Blog blog in blogs)
            {
                IEnumerable<UserThread> dataThreads = _threadRepository.GetUserThreads(blog.UserBlogId);
                foreach (UserThread dataThread in dataThreads)
                {
                    Thread viewThread = ThreadService.GetThread(dataThread.PostId, blog.BlogShortname,
                        dataThread.UserTitle);
                    if (viewThread != null)
                    {
                        viewThreads.Add(viewThread);
                    }
                }
            }
            manager.Threads = viewThreads;
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(manager);
            return Content(json, "application/json");
        }

        public ActionResult GetBlogs()
        {
            IEnumerable<Models.DataModels.UserBlog> blogs =
                _blogRepository.GetUserBlogs(WebSecurity.GetUserId(User.Identity.Name));
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(blogs);
            return Content(json, "application/json");
        }

        public ActionResult GetThreadIds()
        {
            IEnumerable<Blog> blogs =
                BlogFactory.BuildFromDataModel(_blogRepository.GetUserBlogs(WebSecurity.GetUserId(User.Identity.Name)));
            var threadIds = new List<int>();
            foreach (Blog blog in blogs)
            {
                IEnumerable<UserThread> dataThreads = _threadRepository.GetUserThreads(blog.UserBlogId);
                foreach (UserThread dataThread in dataThreads)
                {
                    threadIds.Add(dataThread.UserThreadId);
                }
            }
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(threadIds);
            return Content(json, "application/json");
        }

        public ActionResult GetThread(int threadId)
        {
            UserThread thread = _threadRepository.GetUserThreadById(threadId);
            Models.DataModels.UserBlog blog = _blogRepository.GetUserBlogById(thread.UserBlogId);
            Thread viewThread = ThreadService.GetThread(thread.PostId, blog.BlogShortname, thread.UserTitle);
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(viewThread);
            return Content(json, "application/json");
        }

        [HttpPost]
        public ActionResult ConnectBlog(Blog viewBlog)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Models.DataModels.UserBlog blog = BlogFactory.BuildFromViewModel(viewBlog);
            _blogRepository.InsertUserBlog(blog);
            return RedirectToAction("Threads");
        }

        public ActionResult RemoveBlog(int userBlogId)
        {
            _blogRepository.DeleteUserBlog(userBlogId);
            return RedirectToAction("Threads", "Home");
        }

        [HttpPost]
        public ActionResult TrackThread(string postId, int userBlogId, string userTitle)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            UserThread thread = ThreadFactory.BuildDataModel(postId, userBlogId, userTitle);
            _threadRepository.InsertUserThread(thread);
            return RedirectToAction("Threads", "Home");
        }

        public ActionResult UntrackThread(string postId)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            _threadRepository.DeleteUserThreadByPostId(postId);
            return RedirectToAction("Threads", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult GetLatestNews()
        {
            Thread thread = ThreadService.GetNewsThreads();
            var jsonSerializer = new JavaScriptSerializer();
            string json = jsonSerializer.Serialize(thread);
            return Content(json, "application/json");
        }*/
    }
}