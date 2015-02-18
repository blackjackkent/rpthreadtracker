namespace TumblrThreadTracker.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Interfaces;
    using Models.DataModels;

    public class webpages_MembershipRepository : IRepository<webpages_Membership>
    {
         private readonly IThreadTrackerContext _context;
        private bool _disposed = false;

        public webpages_MembershipRepository(IThreadTrackerContext context)
        {
            _context = context;
        }

        public webpages_Membership Get(int id)
        {
            return _context.webpages_Membership.FirstOrDefault(b => b.UserId == id);
        }

        public IEnumerable<webpages_Membership> Get(Expression<Func<webpages_Membership, bool>> criteria)
        {
            return _context.webpages_Membership.Where(criteria);
        }

        public void Insert(webpages_Membership entity)
        {
            _context.webpages_Membership.Add(entity);
            _context.Commit();
        }

        public void Update(webpages_Membership entity)
        {
            var toUpdate = _context.webpages_Membership.FirstOrDefault(b => b.UserId == entity.UserId);
            if (toUpdate != null)
                toUpdate = entity;
            _context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = _context.webpages_Membership.FirstOrDefault(b => b.UserId == id);
            _context.GetDBSet(typeof(webpages_Membership)).Remove(toUpdate);
            _context.Commit();
        }
    }
}