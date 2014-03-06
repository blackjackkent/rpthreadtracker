using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserThreadRepository : IDisposable
    {
        IEnumerable<UserThread> GetUserThreads();
        IEnumerable<UserThread> GetUserThreads(int userBlogId);
        UserThread GetUserThreadById(int userThreadId);
        void InsertUserThread(UserThread userThread);
        void DeleteUserThread(int userThreadId);
        void UpdateUserThread(UserThread userThread);
        void Save();
    }
}