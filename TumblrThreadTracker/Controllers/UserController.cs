namespace TumblrThreadTracker.Controllers
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Users;
	using Models.RequestModels;

	/// <summary>
	/// Controller class for getting and updating user information
	/// </summary>
	[RedirectOnMaintenance]
	[Authorize]
	public class UserController : ApiController
	{
		private readonly IRepository<User> _userProfileRepository;
		private readonly IUserProfileService _userProfileService;
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserController"/> class
		/// </summary>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="userProfileService">Unity-injected user profile service</param>
		/// <param name="userProfileRepository">Unity-injected user profile repository</param>
		public UserController(IWebSecurityService webSecurityService, IUserProfileService userProfileService, IRepository<User> userProfileRepository)
		{
			_webSecurityService = webSecurityService;
			_userProfileService = userProfileService;
			_userProfileRepository = userProfileRepository;
		}

		/// <summary>
		/// Controller endpoint for getting the currently authenticated user
		/// </summary>
		/// <returns><see cref="UserDto"/> object describing requested user</returns>
		public UserDto Get()
		{
			var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity, _userProfileRepository);
			return user.ToDto();
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
				return Request.CreateResponse(HttpStatusCode.BadRequest, "An account with some or all of this information already exists.");
			}
			_webSecurityService.CreateAccount(request.Username, request.Password, request.Email, _userProfileRepository);
			return new HttpResponseMessage(HttpStatusCode.Created);
		}

		/// <summary>
		/// Controller endpoint for updating the currently authenticated user
		/// </summary>
		/// <param name="user">Request body containing information about user to be updated</param>
		public void Put(UserDto user)
		{
			var currentUser = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity, _userProfileRepository);
			if (currentUser.UserId != user.UserId)
			{
				throw new ArgumentException();
			}

			_userProfileService.Update(user, _userProfileRepository);
		}
	}
}