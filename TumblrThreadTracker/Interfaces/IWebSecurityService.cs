namespace TumblrThreadTracker.Interfaces
{
	using System.Security.Claims;

	using Models.DomainModels.Users;

	public interface IWebSecurityService
	{
		bool ChangePassword(string name, string oldPassword, string newPassword);

		void CreateAccount(string username, string password, string email, IRepository<User> userProfileRepository);

		string GeneratePasswordResetToken(User username);

		User GetCurrentUserFromIdentity(ClaimsIdentity claimsIdentity);

		int? GetCurrentUserIdFromIdentity(ClaimsIdentity identity);

		string GetCurrentUsernameFromIdentity(ClaimsIdentity userIdentity);

		bool ResetPassword(string resetToken, string newPassword);
	}
}