namespace TumblrThreadTracker.Interfaces
{
	using System.Data.Entity;

	using TumblrThreadTracker.Infrastructure;

	public interface IThreadTrackerContext
	{
		DbSet<UserBlog> UserBlogs { get; set; }

		DbSet<UserProfile> UserProfiles { get; set; }

		DbSet<UserThread> UserThreads { get; set; }

		DbSet<UserThreadTag> UserThreadTags { get; set; }

		DbSet<webpages_Membership> webpages_Membership { get; set; }

		void Commit();
	}
}