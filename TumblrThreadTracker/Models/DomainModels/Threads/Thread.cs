namespace TumblrThreadTracker.Models.DomainModels.Threads
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Blogs;
	using TumblrThreadTracker.Models.ServiceModels;

	using Blog = TumblrThreadTracker.Models.DomainModels.Blogs.Blog;

	public class Thread : DomainModel
	{
		public Thread()
		{
		}

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

		public bool IsArchived { get; set; }

		public string PostId { get; set; }

		public List<string> ThreadTags { get; set; }

		public Blog UserBlog { get; set; }

		public int UserBlogId { get; set; }

		public int? UserThreadId { get; set; }

		public string UserTitle { get; set; }

		public string WatchedShortname { get; set; }

		public ThreadDto ToDto(BlogDto blog, IPost post)
		{
			if (post == null)
			{
				return new ThreadDto
					       {
						       BlogShortname = blog.BlogShortname,
						       UserBlogId = blog.UserBlogId.HasValue ? blog.UserBlogId.Value : -1,
						       IsMyTurn = true,
						       LastPostDate = null,
						       LastPostUrl = null,
						       LastPosterShortname = null,
						       PostId = PostId,
						       Type = null,
						       UserThreadId = UserThreadId,
						       UserTitle = UserTitle,
						       WatchedShortname = WatchedShortname,
						       IsArchived = IsArchived,
						       ThreadTags = ThreadTags
					       };
			}

			var dto = new ThreadDto
				          {
					          UserThreadId = UserThreadId,
					          PostId = post.Id.ToString(),
					          UserTitle = UserTitle,
					          Type = post.Type,
					          BlogShortname = blog.BlogShortname,
					          UserBlogId = blog.UserBlogId != null ? blog.UserBlogId.Value : -1,
					          WatchedShortname = WatchedShortname,
					          IsArchived = IsArchived,
					          ThreadTags = ThreadTags
				          };
			if (post.Notes != null && post.Notes.Any(n => n.type == "reblog"))
			{
				Note mostRecentRelevantNote = post.GetMostRecentRelevantNote(blog.BlogShortname, WatchedShortname);

				if (mostRecentRelevantNote != null)
				{
					dto.LastPosterShortname = mostRecentRelevantNote.blog_name;
					dto.LastPostUrl = mostRecentRelevantNote.blog_url + "post/" + mostRecentRelevantNote.post_id;
					dto.LastPostDate = mostRecentRelevantNote.timestamp;
					dto.IsMyTurn = !string.IsNullOrEmpty(WatchedShortname)
						               ? string.Equals(
							               mostRecentRelevantNote.blog_name,
							               WatchedShortname,
							               StringComparison.OrdinalIgnoreCase)
						               : !string.Equals(
							                 mostRecentRelevantNote.blog_name,
							                 blog.BlogShortname,
							                 StringComparison.OrdinalIgnoreCase);
					return dto;
				}
			}

			dto.LastPosterShortname = post.BlogName;
			dto.LastPostUrl = post.PostUrl;
			dto.LastPostDate = post.Timestamp;
			dto.IsMyTurn = !string.IsNullOrEmpty(WatchedShortname)
				               ? string.Equals(post.BlogName, WatchedShortname, StringComparison.OrdinalIgnoreCase)
				               : !string.Equals(post.BlogName, blog.BlogShortname, StringComparison.OrdinalIgnoreCase);

			return dto;
		}
	}
}