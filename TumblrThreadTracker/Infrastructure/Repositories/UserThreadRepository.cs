using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class UserThreadRepository : IRepository<Thread>
    {
        private readonly IThreadTrackerContext _context;

        public UserThreadRepository(IThreadTrackerContext context)
        {
            _context = context;
        }

        public Thread Get(int id)
        {
            return _context.UserThreads.FirstOrDefault(t => t.UserThreadId == id);
        }

        public IEnumerable<Thread> Get(Expression<Func<Thread, bool>> criteria)
        {
            return _context.UserThreads.Where(criteria);
        }

        public void Insert(Thread entity)
        {
            _context.UserThreads.Add(entity);
            _context.Commit();
        }

        public void Update(Thread entity)
        {
            var toUpdate = _context.UserThreads.FirstOrDefault(b => b.UserThreadId == entity.UserThreadId);
            if (toUpdate != null)
            {
                toUpdate.UserBlogId = entity.UserBlogId;
                toUpdate.PostId = entity.PostId;
                toUpdate.UserTitle = entity.UserTitle;
                toUpdate.WatchedShortname = entity.WatchedShortname;
                toUpdate.IsArchived = entity.IsArchived;
            }
            _context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = _context.UserThreads.FirstOrDefault(b => b.UserThreadId == id);
            _context.GetDbSet(typeof (Thread)).Remove(toUpdate);
            _context.Commit();
        }
    }
}