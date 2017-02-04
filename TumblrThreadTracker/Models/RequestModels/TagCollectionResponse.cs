namespace TumblrThreadTracker.Models.RequestModels
{
	using System.Collections.Generic;

	public class TagCollectionResponse
	{
		public string BlogShortname { get; set; }

		public IEnumerable<string> TagCollection { get; set; }

		public int? UserBlogId { get; set; }
	}
}