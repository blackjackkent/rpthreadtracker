namespace RPThreadTracker.Infrastructure.Providers
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Filters;
	using Interfaces;
	using Microsoft.Owin.Security.OAuth;
	using Models.DomainModels.Users;

	/// <summary>
	/// Authorization server provider implementation providing
	/// credential authorization for OWIN server
	/// </summary>
	[ExcludeFromCoverage]
	public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		private readonly IRepository<User> _userProfileRepository;
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleAuthorizationServerProvider"/> class
		/// </summary>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="userProfileRepository">Unity-injected user profile repository</param>
		public SimpleAuthorizationServerProvider(IWebSecurityService webSecurityService, IRepository<User> userProfileRepository)
		{
			_webSecurityService = webSecurityService;
			_userProfileRepository = userProfileRepository;
		}

		/// <inheritdoc cref="OAuthAuthorizationServerProvider"/>
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
			var userId = _webSecurityService.GetUserIdByUsernameAndPassword(context.UserName, context.Password, _userProfileRepository);
			if (userId == null)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}
			var user = _userProfileRepository.GetSingle(u => u.UserId == userId);
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim("username", context.UserName));
			identity.AddClaim(new Claim("userId", user.UserId.ToString()));
			identity.AddClaim(new Claim("role", "user"));
			context.Validated(identity);
		}

		/// <inheritdoc />
		public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();
		}
	}
}