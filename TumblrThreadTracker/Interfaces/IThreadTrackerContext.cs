using System;
using System.Data.Entity;
using TumblrThreadTracker.Infrastructure;

namespace TumblrThreadTracker.Interfaces
{
    public interface IThreadTrackerContext
    {
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<UserBlog> UserBlogs { get; set; }
        DbSet<UserThread> UserThreads { get; set; }
        DbSet<webpages_Membership> webpages_Membership { get; set; }
        void Commit();
    }
}