using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.RequestModels;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class TagController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Thread> _threadRepository;
        private readonly IWebSecurityService _webSecurityService;
        private readonly IBlogService _blogService;
        private readonly IThreadService _threadService;

        public TagController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository,
            IWebSecurityService webSecurityService, IBlogService blogService, IThreadService threadService)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
            _webSecurityService = webSecurityService;
            _blogService = blogService;
            _threadService = threadService;
        }
        public IEnumerable<TagCollectionResponse> Get()
        {
            var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity) User.Identity);
            var tagCollections = new List<TagCollectionResponse>();
            var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository);
            foreach (var blog in blogs)
            {
                var tags = _threadService.GetAllTagsByBlog(blog.UserBlogId, _threadRepository);
                tagCollections.Add(new TagCollectionResponse
                {
                    UserBlogId = blog.UserBlogId,
                    BlogShortname = blog.BlogShortname,
                    TagCollection = tags
                });
            }
            return tagCollections;
        }
    }
}