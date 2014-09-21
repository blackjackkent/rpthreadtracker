using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using System.Web.Http;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;
using WebMatrix.WebData;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace TumblrThreadTracker.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController()
        {
            _userProfileRepository = new UserProfileRepository(new ThreadTrackerContext());
        }

        public int GetUserId()
        {
            return WebSecurity.GetUserId(User.Identity.Name);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Login(LoginModel model, string returnUrl)
        {
            if (ModelState == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            return WebSecurity.Login(model.UserName, model.Password, model.RememberMe)
                ? new HttpResponseMessage(HttpStatusCode.OK)
                : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public HttpResponseMessage LogOff()
        {
            WebSecurity.Logout();
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    var profile = new UserProfile
                    {
                        UserId = WebSecurity.GetUserId(model.UserName),
                        UserName = model.UserName,
                        Email = model.Email
                    };
                    _userProfileRepository.UpdateUserProfile(profile);
                    throw new NotImplementedException();
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            throw new NotImplementedException();
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ForgotPassword(string userName)
        {
            //check user existance
            var user = UserProfile.GetByUsername(userName, _userProfileRepository);
            if (user == null)
                throw new ObjectNotFoundException();
            //generate password token
            string token = WebSecurity.GeneratePasswordResetToken(userName);
            user.ToModel().SendForgotPasswordEmail(token, _userProfileRepository);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage ChangePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            if (!hasLocalAccount)
                throw new InvalidOperationException();
            WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        }

        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}