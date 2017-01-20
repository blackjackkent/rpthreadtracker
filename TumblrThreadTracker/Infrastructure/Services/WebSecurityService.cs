using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Security;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class WebSecurityService : IWebSecurityService
    {
        private readonly IRepository<User> _userProfileRepository;

        public WebSecurityService(IRepository<User> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public void CreateAccount(string username, string password, string email, IRepository<User> userProfileRepository)
        {
            WebSecurity.CreateUserAndAccount(username, password);
            var profile = new User
            {
                UserId = WebSecurity.GetUserId(username),
                UserName =username,
                Email = email,
                LastLogin = DateTime.Now,
                ShowDashboardThreadDistribution = true
            };
            userProfileRepository.Update(profile.UserId, profile);
        }

        public int? GetUserIdByUsernameAndPassword(string username, string password)
        {
            
            var userExistsWithUsername = Membership.Provider.ValidateUser(username, password);
            if (userExistsWithUsername)
            {
                return WebSecurity.GetUserId(username);
            }
            var userByEmail = _userProfileRepository.GetSingle(u => u.Email == username);
            if (userByEmail == null)
            {
                return null;
            }
            var usernameFromEmailIsValid = Membership.Provider.ValidateUser(userByEmail.UserName, password);
            return usernameFromEmailIsValid ? WebSecurity.GetUserId(userByEmail.UserName) : (int?)null;
        }

        public int? GetCurrentUserIdFromIdentity(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
                return null;
            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "userId");
            if (claim == null)
                return null;
            var userId = Int32.Parse(claim.Value);
            return userId;
        }

        public string GetCurrentUsernameFromIdentity(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
                return null;
            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "username");
            if (claim == null)
                return null;
            return claim.Value;
        }

        public User GetCurrentUserFromIdentity(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
                return null;
            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "userId");
            if (claim == null)
                return null;
            var userId = Int32.Parse(claim.Value);
            return _userProfileRepository.GetSingle(u => u.UserId == userId);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(username, oldPassword, newPassword);
        }

        public string GeneratePasswordResetToken(User user)
        {
            return WebSecurity.GeneratePasswordResetToken(user.UserName);
        }

        public bool ResetPassword(string resetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(resetToken, newPassword);
        }
    }
}