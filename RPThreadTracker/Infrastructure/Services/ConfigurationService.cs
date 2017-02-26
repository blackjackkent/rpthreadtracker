namespace RPThreadTracker.Infrastructure.Services
{
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using Filters;
	using Interfaces;

	/// <inheritdoc cref="IConfigurationService"/>
	[ExcludeFromCoverage]
	public class ConfigurationService : IConfigurationService
	{
		/// <inheritdoc cref="IConfigurationService"/>
		public bool MaintenanceMode => ConfigurationManager.AppSettings["MaintenanceMode"] == "true";

		/// <inheritdoc cref="IConfigurationService"/>
		public string NewsBlogShortname => ConfigurationManager.AppSettings["NewsBlogShortname"];

		/// <inheritdoc cref="IConfigurationService"/>
		public string EmailFromAddress => ConfigurationManager.AppSettings["EmailFromAddress"];

		/// <inheritdoc cref="IConfigurationService"/>
		public List<string> AllowedMaintenanceIPs => ConfigurationManager.AppSettings["AllowedMaintenanceIPs"].Split(',').ToList();

		/// <inheritdoc cref="IConfigurationService"/>
		public string TumblrApiKey => ConfigurationManager.AppSettings["TumblrAPIKey"];

		/// <inheritdoc cref="IConfigurationService"/>
		public string SendGridApiKey => ConfigurationManager.AppSettings["SendGridAPIKey"];
	}
}