namespace TumblrThreadTracker.Models.ServiceModels
{
	/// <summary>
	/// Class representing the base object returned by a request to the Tumblr public API
	/// </summary>
	public class ServiceObject
	{
		/// <summary>
		/// Gets or sets the metadata information associated with the response
		/// </summary>
		/// <value>
		/// <see cref="ServiceMeta"/> object containing response metadata
		/// </value>
		public ServiceMeta Meta { get; set; }

		/// <summary>
		/// Gets or sets the data returned from the API
		/// </summary>
		/// <value>
		/// <see cref="ServiceResponse"/> object containing all API-specific results of the request
		/// </value>
		public ServiceResponse Response { get; set; }
	}
}