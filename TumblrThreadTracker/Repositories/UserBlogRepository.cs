using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;

namespace TumblrThreadTracker.Repositories
{
    public class UserBlogRepository : IUserBlogRepository
    {
        private readonly ThreadTrackerContext context;
        private bool disposed;

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
            List<Thread> threads = context.UserThreads.Where(t => t.UserBlogId == userBlogId).ToList();
            context.UserBlogs.Remove(userBlog);
            foreach (Thread thread in threads)
                context.UserThreads.Remove(thread);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}