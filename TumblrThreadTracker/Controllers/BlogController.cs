using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.RequestModels;

namespace TumblrThreadTracker.Controllers
{
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
            //@TODO we should have blogs associated with the returned user object
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity) User.Identity);
            var blogs = _blogService.GetBlogsByUserId(user.UserId, _blogRepository);
            return blogs;
        }

        public void Post(BlogUpdateRequest request)
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            if (request == null)
                throw new ArgumentNullException();
            var dto = new BlogDto
            {
                UserId = user.UserId,
                BlogShortname = request.BlogShortname,
            };
            _blogService.AddNewBlog(dto, _blogRepository);
        }

        public void Put(BlogUpdateRequest request)
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            if (request == null || request.UserBlogId == null)
                throw new ArgumentNullException();
            var dto = new BlogDto
            {
                BlogShortname = request.BlogShortname,
                UserBlogId = request.UserBlogId,
                UserId = user.UserId
            };
            _blogService.UpdateBlog(dto, _blogRepository);
        }

        public void Delete(int userBlogId)
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            var blog = _blogService.GetBlogById(userBlogId, _blogRepository);
            if (blog.UserId != user.UserId)
                return;
            _blogService.DeleteBlog(blog, _blogRepository);
        }
    }
}