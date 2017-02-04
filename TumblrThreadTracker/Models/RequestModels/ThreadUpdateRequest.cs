namespace TumblrThreadTracker.Models.RequestModels
{
	using System.Collections.Generic;

	public class ThreadUpdateRequest
	{
		public string BlogShortname { get; set; }

		public bool IsArchived { get; set; }

		public string PostId { get; set; }

		public List<string> ThreadTags { get; set; }

		public int? UserThreadId { get; set; }

		public string UserTitle { get; set; }

		public string WatchedShortname { get; set; }
	}
}