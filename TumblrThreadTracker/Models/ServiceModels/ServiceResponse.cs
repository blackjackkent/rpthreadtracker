namespace TumblrThreadTracker.Models.ServiceModels
{
	using System.Collections.Generic;

	public class ServiceResponse
	{
		public Blog Blog { get; set; }

		public List<Post> Posts { get; set; }

		public long TotalPosts { get; set; }
	}
}