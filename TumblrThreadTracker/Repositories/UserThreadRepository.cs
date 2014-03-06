using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Repositories
{
    public class UserThreadRepository : IUserThreadRepository
    {
        private bool disposed = false;
        private ThreadTrackerContext context;

        public UserThreadRepository(ThreadTrackerContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserThread> GetUserThreads()
        {
            return context.UserThreads.ToList();
        }

        public IEnumerable<UserThread> GetUserThreads(int userBlogId)
        {
            return context.UserThreads.Where(u => u.UserBlogId == userBlogId);
        }

        public UserThread GetUserThreadById(int userThreadId)
        {
            return context.UserThreads.Find(userThreadId);
        }

        public void InsertUserThread(UserThread userThread)
        {
             context.UserThreads.Add(userThread);
             Save();
        }

        public void DeleteUserThread(int userThreadId)
        {
            UserThread userThread = context.UserThreads.Find(userThreadId);
            context.UserThreads.Remove(userThread);
            Save();
        }

        public void UpdateUserThread(UserThread userThread)
        {
            context.Entry(userThread).State = EntityState.Modified;
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