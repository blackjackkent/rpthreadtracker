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
    public class PublicThreadController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private readonly IUserThreadRepository _threadRepository;

        public PublicThreadController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
        }

        // GET api/<controller>
        public ThreadDto Get(int id)
        {
            return Thread.GetById(id, _blogRepository, _threadRepository);
        }

        public IEnumerable<int> Get(int userId, string blogShortname)
        {
            var ids = new List<int>();
            var blogs = new List<BlogDto>();
            blogs = !string.IsNullOrEmpty(blogShortname) 
                ? Blog.GetBlogsByUserId(userId, _blogRepository).Where(b => b.BlogShortname == blogShortname).ToList() 
                : Blog.GetBlogsByUserId(userId, _blogRepository).ToList();
            foreach (BlogDto blog in blogs)
                ids.AddRange(Thread.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
            return ids;
        }
    }
}