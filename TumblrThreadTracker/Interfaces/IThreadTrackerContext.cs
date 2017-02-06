namespace TumblrThreadTracker.Interfaces
{
	using System.Data.Entity;
	using Infrastructure;

	/// <summary>
	/// Custom EntityFramework context containing RPThreadTracker data structure
	/// </summary>
	public interface IThreadTrackerContext
	{
		/// <summary>
		/// Gets or sets the DbSet containing blog information
		/// </summary>
		/// <value>
		/// <see cref="DbSet"/> collection of <see cref="UserBlog"/> data access values
		/// </value>
		DbSet<UserBlog> UserBlogs { get; set; }

		/// <summary>
		/// Gets or sets the DbSet containing account information
		/// </summary>
		/// <value>
		/// <see cref="DbSet"/> collection of <see cref="UserProfile"/> data access values
		/// </value>
		DbSet<UserProfile> UserProfiles { get; set; }

		/// <summary>
		/// Gets or sets the DbSet containing thread information
		/// </summary>
		/// <value>
		/// <see cref="DbSet"/> collection of <see cref="UserThread"/> data access values
		/// </value>
		DbSet<UserThread> UserThreads { get; set; }

		/// <summary>
		/// Gets or sets the DbSet containing tag information
		/// </summary>
		/// <value>
		/// <see cref="DbSet"/> collection of <see cref="UserThreadTag"/> data access values
		/// </value>
		DbSet<UserThreadTag> UserThreadTags { get; set; }

		/// <summary>
		/// Gets or sets the DbSet containing authentication information
		/// </summary>
		/// <value>
		/// <see cref="DbSet"/> collection of <see cref="WebpagesMemberships"/> data access values
		/// </value>
		DbSet<WebpagesMembership> WebpagesMemberships { get; set; }

		/// <summary>
		/// Saves all changes made to the context datasets
		/// </summary>
		void Commit();
	}
}