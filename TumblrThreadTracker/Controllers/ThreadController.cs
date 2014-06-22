using System;
using System.Web.Http;
using TumblrThreadTracker.Factories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DataModels;
using TumblrThreadTracker.Models.ViewModels;
using TumblrThreadTracker.Repositories;
using TumblrThreadTracker.Services;
using UserBlog = TumblrThreadTracker.Models.DataModels.UserBlog;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class ThreadController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private readonly IUserThreadRepository _threadRepository;

        public ThreadController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
        }

        // GET api/<controller>
        public Thread Get(int id)
        {
            UserThread thread = _threadRepository.GetUserThreadById(id);
            UserBlog blog = _blogRepository.GetUserBlogById(thread.UserBlogId);
            Thread viewThread = ThreadService.GetThread(thread.PostId, blog.BlogShortname, thread.UserTitle);
            return viewThread;
        }

        // POST api/<controller>
        public void Post(string postId, int userBlogId, string userTitle)
        {
            UserThread thread = ThreadFactory.BuildDataModel(postId, userBlogId, userTitle);
            _threadRepository.InsertUserThread(thread);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _threadRepository.DeleteUserThreadByPostId(id);
        }
    }
}