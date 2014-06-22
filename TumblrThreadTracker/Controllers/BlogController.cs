using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Factories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using ViewModels = TumblrThreadTracker.Models.ViewModels;
using DataModels = TumblrThreadTracker.Models.DataModels;
using TumblrThreadTracker.Repositories;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class BlogController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;

        public BlogController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
        }

        public IEnumerable<ViewModels.UserBlog> Get(int id)
        {
            IEnumerable<ViewModels.UserBlog> blogs = BlogFactory.BuildFromDataModel(_blogRepository.GetUserBlogs(id));
            return blogs;
        }

        public void Post(ViewModels.UserBlog viewBlog)
        {
            DataModels.UserBlog blog = BlogFactory.BuildFromViewModel(viewBlog);
            _blogRepository.InsertUserBlog(blog);
        }

        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            _blogRepository.DeleteUserBlog(id);
        }
    }
}