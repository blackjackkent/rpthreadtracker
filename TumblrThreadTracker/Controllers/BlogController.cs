﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.RequestModels;
using TumblrThreadTracker.Repositories;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class BlogController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private static int? _userId;

        public BlogController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _userId = WebSecurity.GetUserId(User.Identity.Name);
        }

        public BlogDto Get(int id)
        {
            return Blog.GetBlogById(id, _blogRepository);
        }

        public IEnumerable<BlogDto> Get()
        {
            if (_userId == null)
            {
                return null;
            }
            IEnumerable<BlogDto> blogs = Blog.GetBlogsByUserId(_userId, _blogRepository);
            return blogs;
        }

        public void Post(BlogUpdateRequest request)
        {
            if (request == null || _userId == null)
            {
                throw new ArgumentNullException();
            }
            var dto = new BlogDto
            {
                UserId = _userId.Value,
                BlogShortname = request.BlogShortname,
            };
            Blog.AddNewBlog(dto, _blogRepository);
        }

        public void Put(BlogUpdateRequest request)
        {
            if (request == null || request.UserBlogId == null)
            {
                throw new ArgumentNullException();
            }
            var dto = new BlogDto
            {
                BlogShortname = request.BlogShortname,
                UserBlogId = request.UserBlogId,
                UserId = _userId != null ? _userId.Value : -1
            };
            Blog.UpdateBlog(dto, _blogRepository);
        }

        public void Delete(int userBlogId)
        {
            _blogRepository.DeleteUserBlog(userBlogId);
        }
    }
}