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
    using System.Linq.Expressions;

    public class UserThreadRepository : IRepository<Thread>
    {
        private bool disposed = false;
        private IThreadTrackerContext context;

        public UserThreadRepository(IThreadTrackerContext context)
        {
            this.context = context;
        }

        public Thread Get(int id)
        {
            return context.UserThreads.FirstOrDefault(t => t.UserThreadId == id);
        }

        public IEnumerable<Thread> Get(Expression<Func<Thread, bool>> criteria)
        {
            return context.UserThreads.Where(criteria);
        }

        public void Insert(Thread entity)
        {
            context.UserThreads.Add(entity);
            context.Commit();
        }

        public void Update(Thread entity)
        {
            var toUpdate = context.UserThreads.FirstOrDefault(b => b.UserThreadId == entity.UserThreadId);
            if (toUpdate != null)
            {
                toUpdate.UserBlogId = entity.UserBlogId;
                toUpdate.PostId = entity.PostId;
                toUpdate.UserTitle = entity.UserTitle;
                toUpdate.WatchedShortname = entity.WatchedShortname;
            }
            context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = context.UserThreads.FirstOrDefault(b => b.UserThreadId == id);
            context.GetDBSet(typeof(Thread)).Remove(toUpdate);
            context.Commit();
        }
    }
}