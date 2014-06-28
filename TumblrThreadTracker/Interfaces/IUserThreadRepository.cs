using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Models.DataModels;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserThreadRepository : IDisposable
    {
        IEnumerable<Thread> GetUserThreads();
        IEnumerable<Thread> GetUserThreads(int userBlogId);
        Thread GetUserThreadById(int userThreadId);
        void InsertUserThread(Thread userThread);
        void DeleteUserThread(int userThreadId);
        void DeleteUserThreadByPostId(string postId);
        void UpdateUserThread(Thread userThread);
        void Save();
    }
}