using System;
using System.Data.Entity;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Infrastructure
{
    public class ThreadTrackerContext : DbContext, IThreadTrackerContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Blog> UserBlogs { get; set; }
        public DbSet<Thread> UserThreads { get; set; }
        public DbSet<WebpagesMembership> WebpagesMembership { get; set; }

        public ThreadTrackerContext()
            : base("name=DefaultConnection")
        {
            Database.SetInitializer<ThreadTrackerContext>(null);
        }

        public void Commit()
        {
            SaveChanges();
        }

        public DbSet GetDbSet(Type type)
        {
            if (type == typeof (UserProfile))
                return UserProfiles;
            if (type == typeof (Blog))
                return UserBlogs;
            if (type == typeof (Thread))
                return UserThreads;
            if (type == typeof (WebpagesMembership))
                return WebpagesMembership;
            return null;
        }
    }
}