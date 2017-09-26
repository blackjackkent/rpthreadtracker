namespace RPThreadTracker.Infrastructure.Services
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Security.Claims;
	using System.Text;
	using System.Threading.Tasks;
	using Filters;
	using Interfaces;
	using Models.DomainModels.Account;
	using Models.DomainModels.Users;
	using WebMatrix.WebData;

	/// <inheritdoc cref="IWebSecurityService"/>
	public class WebSecurityService : IWebSecurityService
	{
		/// <inheritdoc cref="IWebSecurityService"/>
		[ExcludeFromCoverage]
		public bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			return WebSecurity.ChangePassword(username, oldPassword, newPassword);
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		[ExcludeFromCoverage]
		public UserDto CreateAccount(string username, string password, string email, IRepository<User> userProfileRepository)
		{
			WebSecurity.CreateUserAndAccount(username, password);
			var profile = new User
			{
				UserId = WebSecurity.GetUserId(username),
				UserName = username,
				Email = email,
				ShowDashboardThreadDistribution = true
			};
			userProfileRepository.Update(profile.UserId, profile);
			return userProfileRepository.GetSingle(u => u.UserId == profile.UserId).ToDto();
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		[ExcludeFromCoverage]
		public string GeneratePasswordResetToken(UserDto user)
		{
			return WebSecurity.GeneratePasswordResetToken(user.UserName);
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		public UserDto GetCurrentUserFromIdentity(ClaimsIdentity claimsIdentity, IRepository<User> userProfileRepository)
		{
			var claim = claimsIdentity?.Claims.FirstOrDefault(c => c.Type == "userId");
			if (claim == null)
			{
				return null;
			}
			var userId = int.Parse(claim.Value);
			return userProfileRepository.GetSingle(u => u.UserId == userId).ToDto();
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		public int? GetCurrentUserIdFromIdentity(ClaimsIdentity claimsIdentity)
		{
			var claim = claimsIdentity?.Claims.FirstOrDefault(c => c.Type == "userId");
			if (claim == null)
			{
				return null;
			}
			var userId = int.Parse(claim.Value);
			return userId;
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		[ExcludeFromCoverage]
		public int? GetUserIdByUsernameAndPassword(string username, string password, IRepository<User> userProfileRepository)
		{
			var userExistsWithUsername = System.Web.Security.Membership.Provider.ValidateUser(username, password);
			if (userExistsWithUsername)
			{
				return WebSecurity.GetUserId(username);
			}
			var userByEmail = userProfileRepository.GetSingle(u => u.Email == username);
			if (userByEmail == null)
			{
				return null;
			}
			var usernameFromEmailIsValid = System.Web.Security.Membership.Provider.ValidateUser(userByEmail.UserName, password);
			return usernameFromEmailIsValid ? WebSecurity.GetUserId(userByEmail.UserName) : (int?)null;
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		[ExcludeFromCoverage]
		public string ResetPassword(string resetToken)
		{
			var newPassword = GenerateRandomPassword(6);
			var response = WebSecurity.ResetPassword(resetToken, newPassword);
			if (!response)
			{
				throw new InvalidOperationException();
			}
			return newPassword;
		}

		/// <inheritdoc cref="IWebSecurityService"/>
		[ExcludeFromCoverage]
		public async Task SendForgotPasswordEmail(UserDto user, string token, IRepository<Membership> webpagesMembershipRepository, IEmailClient emailClient, IConfigurationService configurationService)
		{
			var isValidToken = IsValidToken(user, token, webpagesMembershipRepository);
			if (!isValidToken)
			{
				throw new Exception("Invalid token");
			}
			var newPassword = ResetPassword(token);
			await SendTemporaryPasswordEmail(user, newPassword, emailClient, configurationService);
		}

		[ExcludeFromCoverage]
		private static bool IsValidToken(UserDto user, string resetToken, IRepository<Membership> webpagesMembershipRepository)
		{
			var record = webpagesMembershipRepository.Get(m => m.UserId == user.UserId && m.PasswordVerificationToken == resetToken);
			return record.Any();
		}

		[ExcludeFromCoverage]
		private static async Task SendTemporaryPasswordEmail(UserDto user, string newPassword, IEmailClient emailClient, IConfigurationService configurationService)
		{
			await emailClient.SendPasswordResetEmail(user.Email, user.UserName, newPassword, configurationService);
		}

		[ExcludeFromCoverage]
		private string GenerateRandomPassword(int length)
		{
			const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
			var chars = new char[length];
			var rd = new Random();
			for (var i = 0; i < length; i++)
			{
				chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
			}
			return new string(chars);
		}
	}
}