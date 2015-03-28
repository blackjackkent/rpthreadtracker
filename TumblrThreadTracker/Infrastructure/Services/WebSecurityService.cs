using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class WebSecurityService : IWebSecurityService
    {
        public void CreateAccount(string username, string password, string email, IRepository<UserProfile> userProfileRepository)
        {
            WebSecurity.CreateUserAndAccount(username, password);
            var profile = new UserProfile
            {
                UserId = WebSecurity.GetUserId(username),
                UserName =username,
                Email = email
            };
            userProfileRepository.Update(profile);
        }

        public bool Login(string username, string password, bool rememberMe = true)
        {
            return WebSecurity.Login(username, password, rememberMe);
        }

        public int GetUserId(string username)
        {
            return WebSecurity.GetUserId(username);
        }

        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            WebSecurity.ChangePassword(username, oldPassword, newPassword);
        }

        public string GeneratePasswordResetToken(string username)
        {
            return WebSecurity.GeneratePasswordResetToken(username);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool ResetPassword(string resetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(resetToken, newPassword);
        }
    }
}