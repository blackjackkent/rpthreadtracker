namespace TumblrThreadTracker.Models.ServiceModels
{
	/// <summary>
	/// Class representing the base object returned by a request to the Tumblr public API
	/// </summary>
	public class ServiceObject
	{
		/// <summary>
		/// Gets or sets the data returned from the API
		/// </summary>
		/// <value>
		/// <see cref="ServiceResponse"/> object containing all API-specific results of the request
		/// </value>
		public ServiceResponse Response { get; set; }
	}
}