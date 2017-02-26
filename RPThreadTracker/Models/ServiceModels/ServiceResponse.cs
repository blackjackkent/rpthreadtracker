namespace RPThreadTracker.Models.ServiceModels
{
	using System.Collections.Generic;

	/// <summary>
	/// Class representing the API-specific response data from a call to the Tumblr API
	/// </summary>
	public class ServiceResponse
	{
		/// <summary>
		/// Gets or sets a collection of post data according to request parameters
		/// </summary>
		/// <value>
		/// List of service model <see cref="Post"/> object
		/// </value>
		public List<Post> Posts { get; set; }
	}
}