namespace TumblrThreadTracker.Models.ServiceModels
{
	/// <summary>
	/// Class representing metadata returned by a request to the Tumblr public API
	/// </summary>
	public class ServiceMeta
	{
		/// <summary>
		/// Gets or sets HTTP reason phrase for request response
		/// </summary>
		/// <value>
		/// The HTTP reason phrase (eg "OK")
		/// </value>
		public string Msg { get; set; }

		/// <summary>
		/// Gets or sets the status code for request response
		/// </summary>
		/// <value>
		/// The 3-digit HTTP status code (eg 200)
		/// </value>
		public int Status { get; set; }
	}
}