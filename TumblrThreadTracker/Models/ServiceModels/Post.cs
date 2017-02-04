namespace TumblrThreadTracker.Models.ServiceModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using TumblrThreadTracker.Interfaces;

	public class Post : ServiceModel, IPost
	{
		public string BlogName { get; set; }

		public bool Bookmarklet { get; set; }

		public string Date { get; set; }

		public string Format { get; set; }

		public long Id { get; set; }

		public bool Liked { get; set; }

		public bool Mobile { get; set; }

		public List<Note> Notes { get; set; }

		public string PostUrl { get; set; }

		public string ReblogKey { get; set; }

		public string SourceTitle { get; set; }

		public string SourceUrl { get; set; }

		public string State { get; set; }

		public List<string> Tags { get; set; }

		public long Timestamp { get; set; }

		public string Title { get; set; }

		public long TotalPosts { get; set; }

		public string Type { get; set; }

		public Note GetMostRecentRelevantNote(string blogShortname, string watchedShortname)
		{
			Note mostRecentRelevantNote;
			if (string.IsNullOrEmpty(watchedShortname)) mostRecentRelevantNote = this.Notes.OrderByDescending(n => n.timestamp).FirstOrDefault(n => n.type == "reblog");
			else
				mostRecentRelevantNote =
					this.Notes.OrderByDescending(n => n.timestamp)
						.FirstOrDefault(
							n =>
								n.type == "reblog"
								&& (string.Equals(n.blog_name, watchedShortname, StringComparison.OrdinalIgnoreCase)
								    || string.Equals(n.blog_name, blogShortname, StringComparison.OrdinalIgnoreCase)));

			return mostRecentRelevantNote;
		}
	}
}