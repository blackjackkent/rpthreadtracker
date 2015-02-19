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
    using System.Linq.Expressions;

    public class UserBlogRepository : IRepository<Blog>
    {
        private readonly IThreadTrackerContext context;
        private bool disposed;

        public UserBlogRepository(IThreadTrackerContext context)
        {
            this.context = context;
        }

        public Blog Get(int id)
        {
            return context.UserBlogs.FirstOrDefault(b => b.UserBlogId == id);
        }

        public IEnumerable<Blog> Get(Expression<Func<Blog, bool>> criteria)
        {
            return context.UserBlogs.Where(criteria);
        }

        public void Insert(Blog entity)
        {
            context.UserBlogs.Add(entity);
            context.Commit();
        }

        public void Update(Blog entity)
        {
            var toUpdate = context.UserBlogs.FirstOrDefault(b => b.UserBlogId == entity.UserBlogId);
            if (toUpdate != null)
            {
                toUpdate.BlogShortname = entity.BlogShortname;
                toUpdate.UserId = entity.UserId;
            }
            context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = context.UserBlogs.FirstOrDefault(b => b.UserBlogId == id);
            context.GetDBSet(typeof(Blog)).Remove(toUpdate);
            context.Commit();
        }
    }
}