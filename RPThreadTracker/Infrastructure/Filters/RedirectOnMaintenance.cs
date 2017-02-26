namespace RPThreadTracker.Infrastructure.Filters
{
	using System.Configuration;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;
	using Interfaces;
	using Microsoft.Practices.Unity;
	using Providers;

	/// <summary>
	/// Attribute used to intercept all tagged controller endpoints
	/// with '503 Service Unavailable' when necessary
	/// </summary>
	public class RedirectOnMaintenance : ActionFilterAttribute
	{
		/// <summary>
		/// Gets or sets Unity-injected configuration service
		/// </summary>
		/// <value>
		/// Wrapper service for application config information
		/// </value>
		[Dependency]
		public IConfigurationService ConfigurationService { get; set; }

		/// <summary>
		/// Adds a check to the normal WebAPI controller processing
		/// and intercepts the response, converting it to a 503
		/// if "MaintenanceMode" is set to 'true' in the web.config
		/// and the user is not from a whitelisted IP
		/// </summary>
		/// <param name="filterContext">Default HttpActionContext object from WebAPI</param>
		public override void OnActionExecuting(HttpActionContext filterContext)
		{
			var allowedIPs = ConfigurationService.AllowedMaintenanceIPs;
			if (ConfigurationService.MaintenanceMode && !allowedIPs.Contains(HttpContextProvider.Current.Request.UserHostAddress))
			{
				filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Temporarily Offline");
			}
			base.OnActionExecuting(filterContext);
		}
	}
}