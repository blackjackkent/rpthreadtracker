using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.RequestModels;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    [Authorize]
    public class BlogController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IBlogService _blogService;
        private readonly IWebSecurityService _webSecurityService;

        public BlogController(IRepository<Blog> userBlogRepository, IWebSecurityService webSecurityService,
            IBlogService blogService)
        {
            _blogRepository = userBlogRepository;
            _webSecurityService = webSecurityService;
            _blogService = blogService;
        }

        public BlogDto Get(int id)
        {
            return _blogService.GetBlogById(id, _blogRepository);
        }

        public IEnumerable<BlogDto> Get()
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository);
            return blogs;
        }

        public void Post(BlogUpdateRequest request)
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            if (request == null)
                throw new ArgumentNullException();
            var dto = new BlogDto
            {
                UserId = userId,
                BlogShortname = request.BlogShortname,
            };
            _blogService.AddNewBlog(dto, _blogRepository);
        }

        public void Put(BlogUpdateRequest request)
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            if (request == null || request.UserBlogId == null)
                throw new ArgumentNullException();
            var dto = new BlogDto
            {
                BlogShortname = request.BlogShortname,
                UserBlogId = request.UserBlogId,
                UserId = userId
            };
            _blogService.UpdateBlog(dto, _blogRepository);
        }

        public void Delete(int userBlogId)
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            var blog = _blogService.GetBlogById(userBlogId, _blogRepository);
            if (blog.UserId != userId)
                return;
            _blogService.DeleteBlog(blog, _blogRepository);
        }
    }
}