using System.Collections.Generic;
using System.Linq;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class BlogService : IBlogService
    {
        public BlogDto GetBlogById(int userBlogId, IRepository<Blog> blogRepository)
        {
            var blog = blogRepository.GetSingle(b => b.UserBlogId == userBlogId);
            return blog.ToDto();
        }

        public IEnumerable<BlogDto> GetBlogsByUserId(int? id, IRepository<Blog> userBlogRepository, bool includeHiatusedBlogs)
        {
            if (id == null)
                return null;
            var blogs = userBlogRepository.Get(b => b.UserId == id);
            if (!includeHiatusedBlogs)
            {
                blogs = blogs.Where(b => !b.OnHiatus);
            }
            return blogs.Select(blog => blog.ToDto()).ToList();
        }

        public BlogDto GetBlogByShortname(string shortname, int userId, IRepository<Blog> userBlogRepository)
        {
            if (shortname == null)
                return null;
            var blog = userBlogRepository.Get(b => b.BlogShortname == shortname && b.UserId == userId).FirstOrDefault();
            return blog != null ? blog.ToDto() : null;
        }

        public void AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository)
        {
            blogRepository.Insert(new Blog(dto));
        }

        public void UpdateBlog(BlogDto dto, IRepository<Blog> blogRepository)
        {
            blogRepository.Update(dto.UserBlogId, new Blog(dto));
        }

        public void DeleteBlog(BlogDto blog, IRepository<Blog> blogRepository)
        {
            blogRepository.Delete(blog.UserBlogId);
        }

	    public bool UserOwnsBlog(int userBlogId, int userId, IRepository<Blog> blogRepository)
	    {
			var userOwnsBlog = blogRepository.Get(t => t.UserBlogId == userBlogId && t.UserId == userId).FirstOrDefault();
			return userOwnsBlog != null;
		}
    }
}