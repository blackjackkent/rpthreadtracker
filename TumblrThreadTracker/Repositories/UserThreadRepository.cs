using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
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

        public IEnumerable<Thread> GetUserThreads()
        {
            return context.UserThreads.ToList();
        }

        public IEnumerable<Thread> GetUserThreads(int? userBlogId, bool isArchived = false)
        {
            if (userBlogId == null)
                return null;
            return context.UserThreads.Where(u => u.UserBlogId == userBlogId && u.IsArchived == isArchived);
        }

        public Thread GetUserThreadById(int userThreadId)
        {
            return context.UserThreads.Find(userThreadId);
        }

        public void InsertUserThread(Thread userThread)
        {
             context.UserThreads.Add(userThread);
             Save();
        }

        public void DeleteUserThread(int userThreadId)
        {
            Thread userThread = context.UserThreads.Find(userThreadId);
            context.UserThreads.Remove(userThread);
            Save();
        }

        public void DeleteUserThreadByPostId(string postId)
        {
            Thread userThread = context.UserThreads.FirstOrDefault(t => t.PostId == postId);
            if (userThread != null)
            {
                context.UserThreads.Remove(userThread);
            }
            Save();
        }

        public void UpdateUserThread(Thread userThread)
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