using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Controllers
{
    public class PublicThreadController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Thread> _threadRepository;

        public PublicThreadController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
        }

        public ThreadDto Get(int id)
        {
            return Thread.GetById(id, _blogRepository, _threadRepository);
        }

        public IEnumerable<int?> Get(int userId, string blogShortname)
        {
            var ids = new List<int?>();
            var blogs = !string.IsNullOrEmpty(blogShortname)
                ? Blog.GetBlogsByUserId(userId, _blogRepository).Where(b => b.BlogShortname == blogShortname).ToList()
                : Blog.GetBlogsByUserId(userId, _blogRepository).ToList();
            foreach (var blog in blogs)
                ids.AddRange(Thread.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
            return ids;
        }
    }
}