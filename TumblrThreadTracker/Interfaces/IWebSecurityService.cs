using System.Security.Claims;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IWebSecurityService
    {
        void CreateAccount(string username, string password, string email, IRepository<User> userProfileRepository);
        void ChangePassword(string name, string oldPassword, string newPassword);
        string GeneratePasswordResetToken(User username);
        bool ResetPassword(string resetToken, string newPassword);
        User GetCurrentUserFromIdentity(ClaimsIdentity claimsIdentity);
        int? GetCurrentUserIdFromIdentity(ClaimsIdentity identity);
        string GetCurrentUsernameFromIdentity(ClaimsIdentity userIdentity);
    }
}