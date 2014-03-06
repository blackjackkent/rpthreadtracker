using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserBlogRepository : IDisposable
    {
        IEnumerable<UserBlog> GetUserBlogs();
        IEnumerable<UserBlog> GetUserBlogs(int userProfileId);
        UserBlog GetUserBlogById(int userBlogId);
        void InsertUserBlog(UserBlog userBlog);
        void DeleteUserBlog(int userBlogId);
        void UpdateUserBlog(UserBlog userBlog);
        void Save();
    }
}