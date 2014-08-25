using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Repositories
{
    public class UserBlogRepository : IUserBlogRepository
    {
        private bool disposed = false;
        private ThreadTrackerContext context;

        public UserBlogRepository(ThreadTrackerContext context)
        {
            this.context = context;
        }

        public IEnumerable<Blog> GetUserBlogs()
        {
            return context.UserBlogs.ToList();
        }

        public IEnumerable<Blog> GetUserBlogs(int? userProfileId)
        {
            return context.UserBlogs.Where(u => u.UserId == userProfileId);
        }

        public Blog GetUserBlogById(int userBlogId)
        {
            return context.UserBlogs.Find(userBlogId);
        }

        public Blog GetUserBlogByShortname(string blogShortname, int userId)
        {
            return context.UserBlogs.FirstOrDefault(b => b.BlogShortname == blogShortname && b.UserId == userId);
        }

        public void InsertUserBlog(Blog userBlog)
        {
             context.UserBlogs.Add(userBlog);
             Save();
        }

        public void DeleteUserBlog(int userBlogId)
        {
            Blog userBlog = context.UserBlogs.Find(userBlogId);
            context.UserBlogs.Remove(userBlog);
            Save();
        }

        public void UpdateUserBlog(Blog userBlog)
        {
            context.Entry(userBlog).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}