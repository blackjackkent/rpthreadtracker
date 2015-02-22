using System.Collections.Generic;
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTracker.Interfaces
{
    public interface IBlogService
    {
        BlogDto GetBlogById(int userBlogId, IRepository<Blog> blogRepository);
        IEnumerable<BlogDto> GetBlogsByUserId(int? id, IRepository<Blog> userBlogRepository);
        BlogDto GetBlogByShortname(string shortname, int userId, IRepository<Blog> userBlogRepository);
        void AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository);
        void UpdateBlog(BlogDto dto, IRepository<Blog> blogRepository);
        void DeleteBlog(BlogDto blog, IRepository<Blog> blogRepository);
    }
}