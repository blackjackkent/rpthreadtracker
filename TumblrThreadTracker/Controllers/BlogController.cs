using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    //[Authorize]
    public class BlogController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private static int? _userId;

        public BlogController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _userId = WebSecurity.GetUserId(User.Identity.Name);
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

        public void Post(BlogDto viewBlog)
        {
            Blog blog = new Blog(viewBlog);
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