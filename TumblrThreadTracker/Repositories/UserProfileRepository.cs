using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private bool disposed = false;
        private ThreadTrackerContext context;

        public UserProfileRepository(ThreadTrackerContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return context.UserProfiles.ToList();
        }

        public UserProfile GetUserProfileById(int userProfileId)
        {
            return context.UserProfiles.Find(userProfileId);
        }

        public void InsertUserProfile(UserProfile userProfile)
        {
             context.UserProfiles.Add(userProfile);
        }

        public void DeleteUserProfile(int userProfileId)
        {
            UserProfile userProfile = context.UserProfiles.Find(userProfileId);
            context.UserProfiles.Remove(userProfile);
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            context.Entry(userProfile).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}