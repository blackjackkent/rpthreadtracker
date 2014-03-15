using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private bool _disposed = false;
        private readonly ThreadTrackerContext _context;

        public UserProfileRepository(ThreadTrackerContext context)
        {
            this._context = context;
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return _context.UserProfiles.ToList();
        }

        public UserProfile GetUserProfileById(int userProfileId)
        {
            return _context.UserProfiles.Find(userProfileId);
        }

        public void InsertUserProfile(UserProfile userProfile)
        {
             _context.UserProfiles.Add(userProfile);
        }

        public void DeleteUserProfile(int userProfileId)
        {
            UserProfile userProfile = _context.UserProfiles.Find(userProfileId);
            _context.UserProfiles.Remove(userProfile);
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            _context.Entry(userProfile).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}