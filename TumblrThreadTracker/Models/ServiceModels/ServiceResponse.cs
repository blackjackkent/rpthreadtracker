namespace TumblrThreadTracker.Models.ServiceModels
{
	using System.Collections.Generic;

	/// <summary>
	/// Class representing the API-specific response data from a call to the Tumblr API
	/// </summary>
	public class ServiceResponse
	{
		/// <summary>
		/// Gets or sets the blog information associated with the request
		/// </summary>
		/// <value>
		/// Service model <see cref="Blog"/> object
		/// </value>
		public Blog Blog { get; set; }

		/// <summary>
		/// Gets or sets a collection of post data according to request parameters
		/// </summary>
		/// <value>
		/// List of service model <see cref="Post"/> object
		/// </value>
		public List<Post> Posts { get; set; }

		/// <summary>
		/// Gets or sets value representing the number of total posts returned by request
		/// </summary>
		/// <value>
		/// Numerical value of total posts returned by request
		/// </value>
		public long TotalPosts { get; set; }
	}
}