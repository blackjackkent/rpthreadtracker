using System;
using System.Collections.Generic;
using TumblrThreadTracker.Domain.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserProfileRepository : IDisposable
    {
        IEnumerable<UserProfile> GetUserProfiles();
        UserProfile GetUserProfileById(int userProfileId);
        UserProfile GetUserProfileByUsername(string username);
        void InsertUserProfile(UserProfile userProfile);
        void DeleteUserProfile(int userProfileId);
        void UpdateUserProfile(UserProfile userProfile);
        void Save();
        bool IsValidPasswordResetToken(int userId, string resetToken);
    }
}