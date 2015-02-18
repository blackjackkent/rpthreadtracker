namespace TumblrThreadTracker.Interfaces
{
    using System;
    using System.Data.Entity;
    using Domain.Blogs;
    using Domain.Threads;
    using Domain.Users;
    using Models.DataModels;

    public interface IThreadTrackerContext
    {
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<Blog> UserBlogs { get; set; }
        DbSet<Thread> UserThreads { get; set; }
        DbSet<webpages_Membership> webpages_Membership { get; set; }
        void Commit();
        DbSet GetDBSet(Type type);
    }
}