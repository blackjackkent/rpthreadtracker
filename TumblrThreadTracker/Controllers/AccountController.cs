namespace TumblrThreadTracker.Controllers
{
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Users;
	using Models.RequestModels;

	/// <summary>
	/// Controller class for account management functions
	/// </summary>
	[RedirectOnMaintenance]
	public class AccountController : ApiController
	{
		private readonly IRepository<User> _userProfileRepository;
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountController"/> class
		/// </summary>
		/// <param name="userProfileRepository">Unity-injected user profile repository</param>
		/// <param name="webSecurityService">Unity-injected web secruty service</param>
		public AccountController(IRepository<User> userProfileRepository, IWebSecurityService webSecurityService)
		{
			_userProfileRepository = userProfileRepository;
			_webSecurityService = webSecurityService;
		}

		/// <summary>
		/// Controller endpoint for creating new UserProfile accounts
		/// </summary>
		/// <param name="request">Request body containing new account information</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		[HttpPost]
		[AllowAnonymous]
		public HttpResponseMessage Post(RegisterRequest request)
		{
			var existingUsername = _userProfileRepository.Get(u => u.UserName == request.Username).Any();
			var existingEmail = _userProfileRepository.Get(u => u.Email == request.Email).Any();

			if (existingUsername || existingEmail)
			{
				return Request.CreateResponse(
					HttpStatusCode.BadRequest,
					"An account with some or all of this information already exists.");
			}

			_webSecurityService.CreateAccount(
				request.Username,
				request.Password,
				request.Email,
				_userProfileRepository);
			return new HttpResponseMessage(HttpStatusCode.Created);
		}
	}
}