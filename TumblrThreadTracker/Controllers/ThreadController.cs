﻿using System;
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
    [Authorize]
    public class ThreadController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private readonly IUserThreadRepository _threadRepository;
        private static int _userId;

        public ThreadController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
            _userId = WebSecurity.GetUserId(User.Identity.Name);
        }

        // GET api/<controller>
        public ThreadDto Get(int id)
        {
            return Thread.GetById(id, _blogRepository, _threadRepository);
        }

        public IEnumerable<int?> Get()
        {
            var ids = new List<int?>();
            var blogs = Blog.GetBlogsByUserId(_userId, _blogRepository);
            foreach (var blog in blogs)
            {
                ids.AddRange(Thread.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
            }
            return ids;
        }

        // POST api/<controller>
        public void Post(long postId, string blogShortname, string userTitle)
        {
            BlogDto blog = Blog.GetBlogByShortname(blogShortname, _userId, _blogRepository);
            ThreadDto dto = new ThreadDto
            {
                UserThreadId = null,
                PostId = postId,
                BlogShortname = blogShortname,
                UserBlogId = blog.UserBlogId,
                UserTitle = userTitle
            };
            Thread.AddNewThread(dto, _threadRepository);
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