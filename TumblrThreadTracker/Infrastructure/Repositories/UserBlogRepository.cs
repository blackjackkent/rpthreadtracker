using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class UserBlogRepository : IRepository<Blog>
    {
        private readonly IThreadTrackerContext _context;

        public UserBlogRepository(IThreadTrackerContext context)
        {
            _context = context;
        }

        public Blog Get(int id)
        {
            return _context.UserBlogs.FirstOrDefault(b => b.UserBlogId == id);
        }

        public IEnumerable<Blog> Get(Expression<Func<Blog, bool>> criteria)
        {
            return _context.UserBlogs.Where(criteria);
        }

        public void Insert(Blog entity)
        {
            _context.UserBlogs.Add(entity);
            _context.Commit();
        }

        public void Update(Blog entity)
        {
            var toUpdate = _context.UserBlogs.FirstOrDefault(b => b.UserBlogId == entity.UserBlogId);
            if (toUpdate != null)
            {
                toUpdate.BlogShortname = entity.BlogShortname;
                toUpdate.UserId = entity.UserId;
            }
            _context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = _context.UserBlogs.FirstOrDefault(b => b.UserBlogId == id);
            _context.GetDbSet(typeof (Blog)).Remove(toUpdate);
            _context.Commit();
        }
    }
}