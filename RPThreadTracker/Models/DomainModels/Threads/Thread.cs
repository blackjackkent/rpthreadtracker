namespace RPThreadTracker.Models.DomainModels.Threads
{
	using System.Collections.Generic;
	using Blogs;
	using Interfaces;

	/// <summary>
	/// Domain Model class representing a user-tracked thread
	/// </summary>
	public class Thread : DomainModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Thread"/> class
		/// </summary>
		public Thread()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Thread"/> class
		/// </summary>
		/// <param name="dto"><see cref="ThreadDto"/> object to convert to domain model</param>
		public Thread(ThreadDto dto)
		{
			UserThreadId = dto.UserThreadId;
			UserBlogId = dto.UserBlogId;
			PostId = dto.PostId;
			UserTitle = dto.UserTitle;
			WatchedShortname = dto.WatchedShortname;
			IsArchived = dto.IsArchived;
			ThreadTags = dto.ThreadTags;
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not the user has marked the blog as archived
		/// </summary>
		/// <value>
		/// True if blog archived, otherwise false
		/// </value>
		public bool IsArchived { get; set; }

		/// <summary>
		/// Gets or sets unique identifier value connecting this thread to a post on Tumblr
		/// </summary>
		/// <value>
		/// String representation of unique numerical identifer of post on Tumblr
		/// </value>
		public string PostId { get; set; }

		/// <summary>
		/// Gets or sets collection of tags user has associated with this thread
		/// </summary>
		/// <value>
		/// List of string tags
		/// </value>
		public List<string> ThreadTags { get; set; }

		/// <summary>
		/// Gets or sets tracked <see cref="Blog"/> object to which this thread belongs
		/// </summary>
		/// <value>
		/// <see cref="Blog"/> object associated with this thread
		/// </value>
		public Blog UserBlog { get; set; }

		/// <summary>
		/// Gets or sets unique identifier of blog in tracker database to which this thread belongs
		/// </summary>
		/// <value>
		/// Integer identifier for user blog
		/// </value>
		public int UserBlogId { get; set; }

		/// <summary>
		/// Gets or sets unique identifier in tracker database for thread object
		/// </summary>
		/// <value>
		/// Integer value of user thread ID, or null if thread is not yet in database
		/// </value>
		public int? UserThreadId { get; set; }

		/// <summary>
		/// Gets or sets title stored in tracker database for this particular thread
		/// </summary>
		/// <value>
		/// String title assigned to thread by user
		/// </value>
		public string UserTitle { get; set; }

		/// <summary>
		/// Gets or sets the shortname of a particular other blog whose
		/// posts should be considered relevant to this thread
		/// </summary>
		/// <value>
		/// String value of blog shortname (http://{shortname}.tumblr.com)
		/// </value>
		public string WatchedShortname { get; set; }

		/// <summary>
		/// Converts <see cref="Thread"/> object to <see cref="ThreadDto"/>
		/// </summary>
		/// <returns><see cref="ThreadDto"/> object corresponding to this thread, hydrated with blog and Tumblr API info</returns>
		public ThreadDto ToDto()
		{
			return new ThreadDto
			{
				BlogShortname = UserBlog?.BlogShortname,
				UserBlogId = UserBlogId,
				IsMyTurn = true,
				LastPostDate = null,
				LastPostUrl = null,
				LastPosterShortname = null,
				PostId = PostId,
				UserThreadId = UserThreadId,
				UserTitle = UserTitle,
				WatchedShortname = WatchedShortname,
				IsArchived = IsArchived,
				ThreadTags = ThreadTags
			};
		}
	}
}