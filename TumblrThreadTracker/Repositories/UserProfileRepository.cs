using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Domain.Users;
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

        public UserProfile GetUserProfileByUsername(string username)
        {
            return _context.UserProfiles.FirstOrDefault(p => p.UserName == username);
        }

        public UserProfile GetUserProfileByEmail(string email)
        {
            return _context.UserProfiles.FirstOrDefault(p => p.Email == email);
        }

        public void InsertUserProfile(UserProfile userProfile)
        {
             _context.UserProfiles.Add(userProfile);
            Save();
        }

        public void DeleteUserProfile(int userProfileId)
        {
            UserProfile userProfile = _context.UserProfiles.Find(userProfileId);
            _context.UserProfiles.Remove(userProfile);
            Save();
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            _context.Entry(userProfile).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool IsValidPasswordResetToken(int userId, string resetToken)
        {
            return _context.webpages_Membership.Any(m => m.UserId == userId && m.PasswordVerificationToken == resetToken);
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