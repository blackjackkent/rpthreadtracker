using System;
using System.Collections.Generic;
using TumblrThreadTracker.Domain.Blogs;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserBlogRepository : IDisposable
    {
        IEnumerable<Blog> GetUserBlogs();
        IEnumerable<Blog> GetUserBlogs(int? userProfileId);
        Blog GetUserBlogById(int userBlogId);
        Blog GetUserBlogByShortname(string blogShortname, int userId);
        void InsertUserBlog(Blog userBlog);
        void DeleteUserBlog(int userBlogId);
        void UpdateUserBlog(Blog userBlog);
        void Save();
    }
}