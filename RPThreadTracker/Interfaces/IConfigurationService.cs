namespace RPThreadTracker.Interfaces
{
	using System.Collections.Generic;

	/// <summary>
	/// Class which facilitates interaction with app settings configuration
	/// </summary>
	public interface IConfigurationService
	{
		/// <summary>
		/// Gets a value indicating whether the site should be displayed in Maintenance Mode
		/// </summary>
		/// <value>
		/// Boolean value indicating whether the site should be displayed in Maintenance Mode
		/// </value>
		bool MaintenanceMode { get; }

		/// <summary>
		/// Gets a value representing the Tumblr shortname associated with the tracker news blog
		/// </summary>
		/// <value>
		/// String value of the shortname
		/// </value>
		string NewsBlogShortname { get; }

		/// <summary>
		/// Gets a value representing the email address from which all tracker emails should be addressed
		/// </summary>
		/// <value>
		/// String value of the email address
		/// </value>
		string EmailFromAddress { get; }

		/// <summary>
		/// Gets a value representing all IP addresses for which maintenance mode should be ignored
		/// </summary>
		/// <value>
		/// List of strings representing the IP addresses
		/// </value>
		List<string> AllowedMaintenanceIPs { get; }

		/// <summary>
		/// Gets a value representing the Tumblr API key associated with this application
		/// </summary>
		/// <value>
		/// String value of the API key
		/// </value>
		string TumblrApiKey { get; }

		/// <summary>
		/// Gets a value representing the Tumblr consumer key associated with this application
		/// </summary>
		/// <value>
		/// String value of the consumer key
		/// </value>
		string TumblrConsumerKey { get; }

		/// <summary>
		/// Gets a value representing the Tumblr consumer secret associated with this application
		/// </summary>
		/// <value>
		/// String value of the consumer secret
		/// </value>
		string TumblrConsumerSecret { get; }

		/// <summary>
		/// Gets a value representing the Tumblr oauth token
		/// for a Tumblr account associated with this application
		/// </summary>
		/// <value>
		/// String value of the oauth token
		/// </value>
		string TumblrOauthToken { get; }

		/// <summary>
		/// Gets a value representing the Tumblr oauth secret
		/// for a Tumblr account associated with this application
		/// </summary>
		/// <value>
		/// String value of the oauth secret
		/// </value>
		string TumblrOauthSecret { get; }

		/// <summary>
		/// Gets a value representing the SendGrid API key associated with this application
		/// </summary>
		/// <value>
		/// String value of the API key
		/// </value>
		string SendGridApiKey { get; }
	}
}