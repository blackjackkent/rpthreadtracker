namespace TumblrThreadTracker.Interfaces
{
	using System.Collections.Generic;
	using Models.ServiceModels;

	/// <summary>
	/// Class representing a Post object as represented by the Tumblr public API
	/// </summary>
	public interface IPost
	{
		/// <summary>
		/// Gets or sets shortname of blog associated with post
		/// </summary>
		/// <value>
		/// String value of blog shortname (http://{shortname}.tumblr.com)
		/// </value>
		string BlogName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether post was created with Tumblr Bookmarklet
		/// </summary>
		/// <value>
		/// True if created with bookmarklet, false if not
		/// </value>
		bool Bookmarklet { get; set; }

		/// <summary>
		/// Gets or sets a string value indicating date at which post was created
		/// </summary>
		/// <value>
		/// String representation of post date
		/// </value>
		string Date { get; set; }

		/// <summary>
		/// Gets or sets a value representing the post format
		/// </summary>
		/// <value>
		/// 'html' or 'markdown' depending on post format
		/// </value>
		string Format { get; set; }

		/// <summary>
		/// Gets or sets the post's unique identifier value
		/// </summary>
		/// <value>
		/// Unique numerical identifer of post
		/// </value>
		long Id { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether an authenticated calling user has liked the post.
		/// </summary>
		/// <value>
		/// True if post has been liked by calling user, false if not
		/// </value>
		bool Liked { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the post was created via Tumblr mobile
		/// </summary>
		/// <value>
		/// True if post was created on mobile, false if not
		/// </value>
		bool Mobile { get; set; }

		/// <summary>
		/// Gets or sets collection of <see cref="Note"/> objects associated with post
		/// </summary>
		/// <value>
		/// Note information (likes and reblogs and replies) for post
		/// </value>
		List<Note> Notes { get; set; }

		/// <summary>
		/// Gets or sets the URL at which the post can be viewed on Tumblr
		/// </summary>
		/// <value>
		/// The post's URL string
		/// </value>
		string PostUrl { get; set; }

		/// <summary>
		/// Gets or sets the value used as an identifier during the reblog process
		/// </summary>
		/// <value>
		/// The posts's unique reblog key string
		/// </value>
		string ReblogKey { get; set; }

		/// <summary>
		/// Gets or sets the title of a post with a content source
		/// </summary>
		/// <value>
		/// String value of the content title
		/// </value>
		string SourceTitle { get; set; }

		/// <summary>
		/// Gets or sets the source URL for a post with a content source
		/// </summary>
		/// <value>
		/// String value of the content source URL
		/// </value>
		string SourceUrl { get; set; }

		/// <summary>
		/// Gets or sets a string representing the current state of the post
		/// </summary>
		/// <value>
		/// 'published', 'queued', 'draft', or 'private'
		/// </value>
		string State { get; set; }

		/// <summary>
		/// Gets or sets a list of tags applied to the post
		/// </summary>
		/// <value>
		/// List of string values representing tags for the post
		/// </value>
		List<string> Tags { get; set; }

		/// <summary>
		/// Gets or sets post timestamp
		/// </summary>
		/// <value>
		/// Epoch timestamp representing time at which post was created
		/// </value>
		long Timestamp { get; set; }

		/// <summary>
		/// Gets or sets title of post on Tumblr
		/// </summary>
		/// <value>
		/// String title applied to Tumblr post
		/// </value>
		string Title { get; set; }

		/// <summary>
		/// Gets or sets value representing the number of total posts returned by request which returned this post
		/// </summary>
		/// <value>
		/// Numerical value of total posts returned by request
		/// </value>
		long TotalPosts { get; set; }

		/// <summary>
		/// Gets or sets value representing post type
		/// </summary>
		/// <value>
		/// 'text', 'quote', 'link', 'answer', 'video', 'audio', 'photo', or 'chat'
		/// </value>
		string Type { get; set; }

		/// <summary>
		/// Processes list of notes attached to post and retrieves the most recent reblog
		/// for tracker purposes
		/// </summary>
		/// <param name="blogShortname">Tracker user's blog shortname (for determining if it is user's turn or not)</param>
		/// <param name="watchedShortname">Optional watched shortname (to specifically track this value's posts as relevant)</param>
		/// <returns>Single <see cref="Note"/> object.</returns>
		Note GetMostRecentRelevantNote(string blogShortname, string watchedShortname);
	}
}