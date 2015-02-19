namespace TumblrThreadTracker.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.Users;
    using Interfaces;
    using Models;

    public class UserProfileRepository : IRepository<UserProfile>
    {
        private readonly IThreadTrackerContext _context;

        public UserProfileRepository(IThreadTrackerContext context)
        {
            _context = context;
        }

        public UserProfile Get(int id)
        {
            return _context.UserProfiles.FirstOrDefault(b => b.UserId == id);
        }

        public IEnumerable<UserProfile> Get(Expression<Func<UserProfile, bool>> criteria)
        {
            return _context.UserProfiles.Where(criteria);
        }

        public void Insert(UserProfile entity)
        {
            _context.UserProfiles.Add(entity);
            _context.Commit();
        }

        public void Update(UserProfile entity)
        {
            var toUpdate = _context.UserProfiles.FirstOrDefault(b => b.UserId == entity.UserId);
            if (toUpdate != null)
            {
                toUpdate.UserName = entity.UserName;
                toUpdate.Email = entity.Email;
                toUpdate.Password = entity.Password;
            }
            _context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = _context.UserProfiles.FirstOrDefault(b => b.UserId == id);
            _context.GetDBSet(typeof(UserProfile)).Remove(toUpdate);
            _context.Commit();
        }
    }
}