using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IWebSecurityService
    {
        void CreateAccount(string username, string password, string email, IRepository<UserProfile> userProfileRepository);
        bool Login(string username, string password, bool rememberMe = true);
        int GetUserId(string username);
        void ChangePassword(string name, string oldPassword, string newPassword);
        string GeneratePasswordResetToken(string username);
        void Logout();
    }
}