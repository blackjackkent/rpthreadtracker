using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class UserBlogRepository : BaseRepository<Blog, BlogDto, UserBlog>
    {
        private readonly IThreadTrackerContext _context;
        private readonly IDbSet<UserBlog> _dbSet; 
        protected override IThreadTrackerContext Context
        {
            get { return _context; }
        }

        protected override IDbSet<UserBlog> DbSet
        {
            get { return _dbSet; }
        }

        public UserBlogRepository(IThreadTrackerContext context)
        {
            _context = context;
            _dbSet = context.UserBlogs;
        }
    }
}