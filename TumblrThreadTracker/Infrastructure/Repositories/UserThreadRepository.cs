using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class UserThreadRepository : BaseRepository<Thread, ThreadDto, UserThread>
    {
        private readonly IThreadTrackerContext _context;
        private readonly IDbSet<UserThread> _dbSet; 
        protected override IThreadTrackerContext Context
        {
            get { return _context; }
        }

        protected override IDbSet<UserThread> DbSet
        {
            get { return _dbSet; }
        }

        public UserThreadRepository(IThreadTrackerContext context)
        {
            _context = context;
            _dbSet = context.UserThreads;
        }
    }
}