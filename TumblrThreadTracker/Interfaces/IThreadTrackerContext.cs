using System;
using System.Data.Entity;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IThreadTrackerContext
    {
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<Blog> UserBlogs { get; set; }
        DbSet<Thread> UserThreads { get; set; }
        DbSet<WebpagesMembership> WebpagesMembership { get; set; }
        void Commit();
        DbSet GetDbSet(Type type);
    }
}