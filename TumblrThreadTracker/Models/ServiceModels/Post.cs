namespace TumblrThreadTracker.Models.ServiceModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Interfaces;

	/// <inheritdoc cref="IPost"/>
	public class Post : IPost
	{
		/// <inheritdoc cref="IPost"/>
		public string BlogName { get; set; }

		/// <inheritdoc cref="IPost"/>
		public bool Bookmarklet { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string Date { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string Format { get; set; }

		/// <inheritdoc cref="IPost"/>
		public long Id { get; set; }

		/// <inheritdoc cref="IPost"/>
		public bool Liked { get; set; }

		/// <inheritdoc cref="IPost"/>
		public bool Mobile { get; set; }

		/// <inheritdoc cref="IPost"/>
		public List<Note> Notes { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string PostUrl { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string ReblogKey { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string SourceTitle { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string SourceUrl { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string State { get; set; }

		/// <inheritdoc cref="IPost"/>
		public List<string> Tags { get; set; }

		/// <inheritdoc cref="IPost"/>
		public long Timestamp { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string Title { get; set; }

		/// <inheritdoc cref="IPost"/>
		public long TotalPosts { get; set; }

		/// <inheritdoc cref="IPost"/>
		public string Type { get; set; }

		/// <inheritdoc cref="IPost"/>
		public Note GetMostRecentRelevantNote(string blogShortname, string watchedShortname)
		{
			Note mostRecentRelevantNote;
			if (Notes == null || Notes.All(n => n.Type != "reblog"))
			{
				return null;
			}
			if (string.IsNullOrEmpty(watchedShortname))
			{
				mostRecentRelevantNote = Notes.OrderByDescending(n => n.Timestamp).FirstOrDefault(n => n.Type == "reblog");
			}
			else
			{
				mostRecentRelevantNote = Notes.OrderByDescending(n => n.Timestamp).FirstOrDefault(n =>
								n.Type == "reblog" && (string.Equals(n.BlogName, watchedShortname, StringComparison.OrdinalIgnoreCase)
									|| string.Equals(n.BlogName, blogShortname, StringComparison.OrdinalIgnoreCase)));
			}

			return mostRecentRelevantNote;
		}
	}
}