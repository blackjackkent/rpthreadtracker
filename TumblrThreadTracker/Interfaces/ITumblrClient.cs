namespace TumblrThreadTracker.Interfaces
{
	using System.Collections.Generic;

	using TumblrThreadTracker.Models.ServiceModels;

	public interface ITumblrClient
	{
		IEnumerable<Post> GetNewsPosts(int? count = null);

		IPost GetPost(string postId, string blogShortname);
	}
}