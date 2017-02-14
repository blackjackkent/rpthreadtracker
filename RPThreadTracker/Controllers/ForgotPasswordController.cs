namespace RPThreadTracker.Controllers
{
	using System.Data.Entity.Core;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Account;
	using Models.DomainModels.Users;

	/// <summary>
	/// Controller class for handling forgot password flow
	/// </summary>
	[RedirectOnMaintenance]
	public class ForgotPasswordController : ApiController
	{
		private readonly IEmailService _emailService;
		private readonly IRepository<User> _userProfileRepository;
		private readonly IRepository<Membership> _webpagesMembershipRepository;
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ForgotPasswordController"/> class
		/// </summary>
		/// <param name="userProfileRepository">Unity-injected user profile repository</param>
		/// <param name="webpagesMembershipRepository">Unity-injected WebpagesMembership repository</param>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="emailService">Unity-injected email service</param>
		public ForgotPasswordController(IRepository<User> userProfileRepository, IRepository<Membership> webpagesMembershipRepository, IWebSecurityService webSecurityService, IEmailService emailService)
		{
			_userProfileRepository = userProfileRepository;
			_webpagesMembershipRepository = webpagesMembershipRepository;
			_webSecurityService = webSecurityService;
			_emailService = emailService;
		}

		/// <summary>
		/// Controller endpoing requesting a new password be assigned to account
		/// </summary>
		/// <param name="usernameOrEmail">
		/// Identifier used to find account to be reset
		/// </param>
		/// <returns>HttpResponseMessage indicating success of request</returns>
		[HttpPost]
		[AllowAnonymous]
		public async Task<HttpResponseMessage> Post([FromBody] string usernameOrEmail)
		{
			var username = usernameOrEmail;
			var user = _userProfileRepository.GetSingle(u => u.UserName == username)
						?? _userProfileRepository.GetSingle(u => u.Email == username);
			if (user == null || username == null)
			{
				throw new ObjectNotFoundException();
			}
			var token = _webSecurityService.GeneratePasswordResetToken(user);
			await _webSecurityService.SendForgotPasswordEmail(user, token, _webpagesMembershipRepository, _emailService);
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}