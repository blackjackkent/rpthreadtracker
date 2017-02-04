namespace TumblrThreadTracker.Models.ServiceModels
{
	/// <summary>
	/// Class representing a Blog object as represented by the Tumblr public API
	/// </summary>
	public class Blog
	{
		/// <summary>
		/// Gets or sets a value indicating whether this blog allows asks
		/// </summary>
		/// <value>
		/// True if blog allows asks, otherwise false
		/// </value>
		public bool Ask { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this blog allows anonymous asks
		/// </summary>
		/// <value>
		/// True if anonymous asks enabled, otherwise false
		/// </value>
		public bool AskAnon { get; set; }

		/// <summary>
		/// Gets or sets a value representing the blog's description
		/// </summary>
		/// <value>
		/// String value of description set by user
		/// </value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value representing how many posts have been liked by this blog
		/// </summary>
		/// <value>
		/// Numerical value representing cumulative likes by this blog
		/// </value>
		public long Likes { get; set; }

		/// <summary>
		/// Gets or sets value of blog shortname
		/// </summary>
		/// <value>
		/// String value of blog shortname (http://{shortname}.tumblr.com)
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating the number of posts made on this blog
		/// </summary>
		/// <value>
		/// Numerical value representing cumulative number of posts on blog
		/// </value>
		public long Posts { get; set; }

		/// <summary>
		/// Gets or sets title of blog object
		/// </summary>
		/// <value>
		/// String value representing blog title on Tumblr
		/// </value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets timestamp value representing most recent post on the blog
		/// </summary>
		/// <value>
		/// Epoch timestamp of last post on blog
		/// </value>
		public long Updated { get; set; }
	}
}