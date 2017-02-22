namespace RPThreadTracker.Interfaces
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Models.DomainModels.Account;
	using Models.DomainModels.Users;
	using WebMatrix.WebData;

	/// <summary>
	/// Injectable wrapper class for <see cref="WebSecurity"/> functionality
	/// with additional tracker-specific authentication functions
	/// </summary>
	public interface IWebSecurityService
	{
		/// <summary>
		/// Changes the password for the specified user.
		/// </summary>
		/// <param name="username">Username of account to be changed</param>
		/// <param name="oldPassword">The current password for the user</param>
		/// <param name="newPassword">The new password to assign to the account</param>
		/// <returns>True if password successfully changed, otherwise false</returns>
		bool ChangePassword(string username, string oldPassword, string newPassword);

		/// <summary>
		/// Creates a new membership account using the specified user name and password.
		/// </summary>
		/// <param name="username">Username of account to be created</param>
		/// <param name="password">Password to be assigned to account</param>
		/// <param name="email">Email address to be associated with account</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns><see cref="UserDto" /> object created by insert</returns>
		UserDto CreateAccount(string username, string password, string email, IRepository<User> userProfileRepository);

		/// <summary>
		/// Generates a password reset token that can be sent to a user in email.
		/// </summary>
		/// <param name="user">Username of the account that is being reset</param>
		/// <returns>String token to be sent to user</returns>
		string GeneratePasswordResetToken(UserDto user);

		/// <summary>
		/// Gets <see cref="User"/> object based on authenticated identity
		/// </summary>
		/// <param name="claimsIdentity">Identity of a user based on their authentication claims</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns><see cref="User"/> object associated with the identity</returns>
		UserDto GetCurrentUserFromIdentity(ClaimsIdentity claimsIdentity, IRepository<User> userProfileRepository);

		/// <summary>
		/// Gets user ID based on authenticated identity
		/// </summary>
		/// <param name="claimsIdentity">Identity of a user based on their authentication claims</param>
		/// <returns>Integer identifier for user associated with the identity</returns>
		int? GetCurrentUserIdFromIdentity(ClaimsIdentity claimsIdentity);

		/// <summary>
		/// Gets user ID based on passed credentials
		/// </summary>
		/// <param name="username">Username credential passed to authenticate</param>
		/// <param name="password">Password credential passed to authenticate</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns>Integer identifier for user associated with the identity</returns>
		int? GetUserIdByUsernameAndPassword(string username, string password, IRepository<User> userProfileRepository);

		/// <summary>
		/// Generates a new user account password using a password reset token
		/// and assigns it to account
		/// </summary>
		/// <param name="resetToken">String token which verifies password reset</param>
		/// <returns>Temporary password generated for account</returns>
		string ResetPassword(string resetToken);

		/// <summary>
		/// Generates a random string to be used as a temporary password
		/// </summary>
		/// <param name="length">Length of string to generate</param>
		/// <returns>String of random characters</returns>
		string GenerateRandomPassword(int length);

		/// <summary>
		/// Sends an email to the user containing a temporary password to log into the site
		/// </summary>
		/// <param name="user">User account for which to update password</param>
		/// <param name="token">Password reset token string</param>
		/// <param name="webpagesMembershipRepository">Repository object containing database connection</param>
		/// <param name="emailClient">Service responsible for constructing and sending email message</param>
		/// <param name="configurationService">Wrapper service for app config information</param>
		/// <returns>Task object for async handling</returns>
		Task SendForgotPasswordEmail(UserDto user, string token, IRepository<Membership> webpagesMembershipRepository, IEmailClient emailClient, IConfigurationService configurationService);
	}
}