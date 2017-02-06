namespace TumblrThreadTracker.Controllers
{
	using System.Net;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Users;
	using Models.RequestModels;

	/// <summary>
	/// Controller class for changing account passwords
	/// </summary>
	[RedirectOnMaintenance]
	[Authorize]
	public class ChangePasswordController : ApiController
	{
		private readonly IWebSecurityService _webSecurityService;
		private readonly IRepository<User> _userProfileRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangePasswordController"/> class
		/// </summary>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="userProfileRepository">Unity-injected user profile repository</param>
		public ChangePasswordController(IWebSecurityService webSecurityService, IRepository<User> userProfileRepository)
		{
			_webSecurityService = webSecurityService;
			_userProfileRepository = userProfileRepository;
		}

		/// <summary>
		/// Controller endpoint for changing password of currently authenticated user
		/// </summary>
		/// <param name="model">Request body containing information about password change</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		[HttpPost]
		public HttpResponseMessage ChangePassword(ChangePasswordRequest model)
		{
			var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity, _userProfileRepository);
			if (user == null)
			{
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
			var success = _webSecurityService.ChangePassword(user.UserName, model.OldPassword, model.NewPassword);
			return success ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.BadRequest);
		}
	}
}