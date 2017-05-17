namespace RPThreadTracker.Models.DomainModels.Threads
{
	using System;
	using System.Collections.Generic;
	using Interfaces;

	/// <summary>
	/// DTO object for transferring <see cref="Thread" /> data
	/// </summary>
	public class ThreadDto : IDto<Thread>
	{
		/// <summary>
		/// Gets or sets shortname (username) value of Tumblr blog being tracked
		/// </summary>
		/// <value>
		/// String value of shortname
		/// </value>
		public string BlogShortname { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the user has marked the blog as archived on the tracker
		/// </summary>
		/// <value>
		/// True if blog archived, otherwise false
		/// </value>
		public bool IsArchived { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not it should be considered the requesting user's turn on this thread.
		/// </summary>
		/// <value>
		/// True if user was not the last poster on the thread on Tumblr
		/// OR the WatchedShortname value is not null and the watched user was the last poster
		/// Otherwise false
		/// </value>
		public bool IsMyTurn { get; set; }

		/// <summary>
		/// Gets or sets a timestamp representing the last relevant time the thread was updated on Tumblr.
		/// </summary>
		/// <value>
		/// Epoch timestamp representing last relevant post to thread
		/// </value>
		public long? LastPostDate { get; set; }

		/// <summary>
		/// Gets or sets shortname (username) value associated with last relevant post to the thread on Tumblr
		/// </summary>
		/// <value>
		/// String shortname of the poster of the last relevant update to the thread
		/// </value>
		public string LastPosterShortname { get; set; }

		/// <summary>
		/// Gets or sets the Tumblr URL of the last relevant post to the thread
		/// </summary>
		/// <value>
		/// String URL of the last relevant post to the thread on Tumblr
		/// </value>
		public string LastPostUrl { get; set; }

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
		/// Gets or sets unique identifier in tracker database of blog to which this thread belongs
		/// </summary>
		/// <value>
		/// Integer identifier for user blog
		/// </value>
		public int UserBlogId { get; set; }

		/// <summary>
		/// Gets or sets unique identifier for thread object in tracker database
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
		/// Gets or sets the datetime at which this thread was marked
		/// by the user as having been queued on Tumblr.
		/// </summary>
		/// <value>
		/// Datetime at which the user marked the thread queued,
		/// or null if it is not presently in the queue.
		/// </value>
		public DateTime? MarkedQueued { get; set; }

		/// <inheritdoc cref="IDto{TModel}"/>
		public Thread ToModel()
		{
			return new Thread(this);
		}
	}
}