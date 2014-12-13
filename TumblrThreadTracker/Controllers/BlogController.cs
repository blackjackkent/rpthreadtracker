using System;
using System.Collections.Generic;
using System.Linq;
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

        public BlogController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
        }

        public BlogDto Get(int id)
        {
            return Blog.GetBlogById(id, _blogRepository);
        }

        public IEnumerable<BlogDto> Get()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            IEnumerable<BlogDto> blogs = Blog.GetBlogsByUserId(userId, _blogRepository);
            return blogs;
        }

        public void Post(BlogUpdateRequest request)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            if (request == null)
            {
                throw new ArgumentNullException();
            }
            var dto = new BlogDto
            {
                UserId = userId,
                BlogShortname = request.BlogShortname,
            };
            Blog.AddNewBlog(dto, _blogRepository);
        }

        public void Put(BlogUpdateRequest request)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            if (request == null || request.UserBlogId == null)
            {
                throw new ArgumentNullException();
            }
            var dto = new BlogDto
            {
                BlogShortname = request.BlogShortname,
                UserBlogId = request.UserBlogId,
                UserId = userId
            };
            Blog.UpdateBlog(dto, _blogRepository);
        }

        public void Delete(int userBlogId)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            var blog = Blog.GetBlogById(userBlogId, _blogRepository);
            if (blog.UserId != userId)
                return;
            Blog.DeleteBlog(blog, _blogRepository);
        }
    }
}