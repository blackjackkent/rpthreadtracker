namespace TumblrThreadTracker.Controllers
{
	using System.Net;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Web.Http;

	using Infrastructure.Filters;
	using Interfaces;
	using Models.RequestModels;

	/// <summary>
	/// Controller class for changing account passwords
	/// </summary>
	[RedirectOnMaintenance]
	[Authorize]
	public class ChangePasswordController : ApiController
	{
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangePasswordController"/> class
		/// </summary>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		public ChangePasswordController(IWebSecurityService webSecurityService)
		{
			_webSecurityService = webSecurityService;
		}

		/// <summary>
		/// Controller endpoint for changing password of currently authenticated user
		/// </summary>
		/// <param name="model">Request body containing information about password change</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		[HttpPost]
		public HttpResponseMessage ChangePassword(ChangePasswordRequest model)
		{
			var username = _webSecurityService.GetCurrentUsernameFromIdentity((ClaimsIdentity)User.Identity);
			var success = _webSecurityService.ChangePassword(username, model.OldPassword, model.NewPassword);
			return success ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.BadRequest);
		}
	}
}