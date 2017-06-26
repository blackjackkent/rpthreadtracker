namespace RPThreadTracker.Interfaces
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Wrapper class for HTTP client connection to Tumblr API
	/// </summary>
	public interface ITumblrClient
	{
		/// <summary>
		/// Gets most recent #news posts from the Tracker blog on Tumblr
		/// </summary>
		/// <param name="count">Number of posts to retrieve</param>
		/// <returns>List of Tumblr API <see cref="IPost"/> objects</returns>
		Task<IEnumerable<IPost>> GetNewsPosts(int? count = null);

		/// <summary>
		/// Gets an individual post from Tumblr API
		/// </summary>
		/// <param name="postId">String representation of Tumblr's unique numerical post ID</param>
		/// <param name="blogShortname">Shortname of tracked blog to which post belongs</param>
		/// <returns>Tumblr API <see cref="IPost"/> object</returns>
		Task<IPost> GetPost(string postId, string blogShortname);
	}
}