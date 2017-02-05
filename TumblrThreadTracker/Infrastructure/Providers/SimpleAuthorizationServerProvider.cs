namespace TumblrThreadTracker.Infrastructure.Providers
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.Owin.Security.OAuth;
	using Repositories;
	using Services;

	/// <summary>
	/// Authorization server provider implementation providing
	/// credential authorization for OWIN server
	/// </summary>
	public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		/// <inheritdoc />
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

			// @TODO inject?
			using (var trackerContext = new RPThreadTrackerEntities())
			{
				var userRepository = new UserProfileRepository(trackerContext);
				var webSecurityService = new WebSecurityService(userRepository);
				var userId = webSecurityService.GetUserIdByUsernameAndPassword(context.UserName, context.Password);
				if (userId == null)
				{
					context.SetError("invalid_grant", "The user name or password is incorrect.");
					return;
				}

				var user = userRepository.GetSingle(u => u.UserId == userId);
				var identity = new ClaimsIdentity(context.Options.AuthenticationType);
				identity.AddClaim(new Claim("username", context.UserName));
				identity.AddClaim(new Claim("userId", user.UserId.ToString()));
				identity.AddClaim(new Claim("role", "user"));
				context.Validated(identity);
			}
		}

		/// <inheritdoc />
		public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();
		}
	}
}