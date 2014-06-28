using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;
using TumblrThreadTracker.Services;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    //[Authorize]
    public class ThreadController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private readonly IUserThreadRepository _threadRepository;
        private static int _userId;

        public ThreadController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
           // _userId = WebSecurity.GetUserId(User.Identity.Name);
            _userId = 7;
        }

        // GET api/<controller>
        public ThreadDto Get(int id)
        {
            return Thread.GetById(id, _blogRepository, _threadRepository);
        }

        public IEnumerable<int> Get()
        {
            var ids = new List<int>();
            var blogs = Blog.GetBlogsByUserId(_userId, _blogRepository);
            foreach (var blog in blogs)
            {
                ids.AddRange(Thread.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
            }
            return ids;
        }

        // POST api/<controller>
        public void Post(string postId, int userBlogId, string userTitle)
        {
           /* UserThread thread = ThreadFactory.BuildDataModel(postId, userBlogId, userTitle);
            _threadRepository.InsertUserThread(thread);*/
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