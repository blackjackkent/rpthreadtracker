using System;
using System.Collections.Generic;
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

namespace TumblrThreadTracker.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController()
        {
            _userProfileRepository = new UserProfileRepository(new ThreadTrackerContext());
        }

        //
        // GET: /Account/Login

        public int GetUserId()
        {
            return WebSecurity.GetUserId(User.Identity.Name);
        }

        //
        // POST: /Account/Login

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

        //
        // POST: /Account/LogOff

        [HttpPost]
        public HttpResponseMessage LogOff()
        {
            WebSecurity.Logout();
            throw new NotImplementedException();
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public HttpResponseMessage Register()
        {
            throw new NotImplementedException();
        }

        //
        // POST: /Account/Register

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

        [AllowAnonymous]
        public HttpResponseMessage ForgotPassword()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ForgotPassword(string userName)
        {
            //check user existance
            MembershipUser user = Membership.GetUser(userName);
            if (user == null)
            {
                throw new NotImplementedException();
            }
            //generate password token
            string token = WebSecurity.GeneratePasswordResetToken(userName);
            //create url with above token
            //var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = userName, rt = token }, "http") + "'>Reset Password</a>";
            throw new NotImplementedException();
            //get user emailid
            var db = new ThreadTrackerContext();
            string emailid = (from i in db.UserProfiles
                where i.UserName == userName
                select i.Email).FirstOrDefault();
            //send mail
            string subject = "RPThreadTracker ~ Password Reset";
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<p>Hello,</p>");
            bodyBuilder.Append("<p>Please use the link below to reset your password on RPThreadTracker:</p>");
            //bodyBuilder.Append("<p>" + resetLink + "</p>");
            bodyBuilder.Append("<p>Thanks and have a great day!");
            bodyBuilder.Append("<p>~Tracker-mun</p>");
            string body = bodyBuilder.ToString();
            try
            {
                SendEmail(emailid, subject, body);
                // TempData["Message"] = "Thanks! A link to reset your password should arrive in your email inbox within the next few minutes!";
            }
            catch (Exception ex)
            {
                //TempData["Message"] = "Error occured while sending email." + ex.Message;
            }
        }

        [AllowAnonymous]
        public HttpResponseMessage ResetPassword(string un, string rt)
        {
            var db = new ThreadTrackerContext();
            //TODO: Check the un and rt matching and then perform following
            //get userid of received username
            int userid = (from i in db.UserProfiles
                where i.UserName == un
                select i.UserId).FirstOrDefault();
            //check userid and token matches
            bool any = (from j in db.webpages_Membership
                where (j.UserId == userid)
                      && (j.PasswordVerificationToken == rt)
                //&& (j.PasswordVerificationTokenExpirationDate < DateTime.Now)
                select j).Any();

            if (any)
            {
                //generate random password
                string newpassword = GenerateRandomPassword(6);
                //reset password
                bool response = WebSecurity.ResetPassword(rt, newpassword);
                if (response)
                {
                    //get user emailid to send password
                    string emailid = (from i in db.UserProfiles
                        where i.UserName == un
                        select i.Email).FirstOrDefault();
                    //send email
                    string subject = "RPThreadTracker ~ New Temporary Password";
                    var bodyBuilder = new StringBuilder();
                    bodyBuilder.Append("<p>Hello,</p>");
                    bodyBuilder.Append("<p>Below is your autogenerated temporary password for RPThreadTracker:</p>");
                    bodyBuilder.Append("<p>" + newpassword + "</p>");
                    bodyBuilder.Append(
                        "<p>Use this password to log into the tracker; be sure to change your password to something secure once you are logged in.</p>");
                    bodyBuilder.Append("<p>Thanks, and have a great day!</p>");
                    bodyBuilder.Append("<p>~Tracker-mun</p>");
                    string body = bodyBuilder.ToString();
                    throw new NotImplementedException();
                    try
                    {
                        SendEmail(emailid, subject, body);
                        //TempData["Message"] = "Mail Sent.";
                    }
                    catch (Exception ex)
                    {
                        //TempData["Message"] = "Error occured while sending email." + ex.Message;
                    }

                    //display message
                    //TempData["Message"] ="<h2>You have successfully reset your password.</h2> <h3>Your new temporary password should appear in your email inbox within a few minutes.</h3> <p>Be sure to change your password to something secure once you have logged in.</p>";
                }
            }
            else
            {
                throw new NotImplementedException();
                //TempData["Message"] = "You have reached this page via an invalid URL.";
            }
            throw new NotImplementedException();
        }

        private static string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            var chars = new char[length];
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        private static void SendEmail(string emailid, string subject, string body)
        {
            var client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "mail.rpthreadtracker.com";
            client.Port = 25;

            client.UseDefaultCredentials = false;
            var credentials = new NetworkCredential("postmaster@rpthreadtracker.com", "***REMOVED***");
            client.Credentials = credentials;

            var msg = new MailMessage();
            msg.From = new MailAddress("postmaster@rpthreadtracker.com");
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        public HttpResponseMessage Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (
                    var scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions {IsolationLevel = IsolationLevel.Serializable}))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }
            throw new NotImplementedException();
            //return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public HttpResponseMessage Manage(ManageMessageId? message)
        {
            /*ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();*/
            throw new NotImplementedException();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        public HttpResponseMessage Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            throw new NotImplementedException();
            /*ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);*/
            throw new NotImplementedException();
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ExternalLogin(string provider, string returnUrl)
        {
            //return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            throw new NotImplementedException();
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public HttpResponseMessage ExternalLoginCallback(string returnUrl)
        {
            /*AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }*/
            throw new NotImplementedException();
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            /*string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (ThreadTrackerContext db = new ThreadTrackerContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);*/
            throw new NotImplementedException();
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public HttpResponseMessage ExternalLoginFailure()
        {
            // return View();
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        public HttpResponseMessage ExternalLoginsList(string returnUrl)
        {
            /*ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);*/
            throw new NotImplementedException();
        }

        public HttpResponseMessage RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            var externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            /* ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);*/
            throw new NotImplementedException();
        }

        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private HttpResponseMessage RedirectToLocal(string returnUrl)
        {
            /*if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }*/
            throw new NotImplementedException();
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

        internal class ExternalLoginResult : HttpResponseMessage
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            /* public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }*/
        }

        #endregion
    }
}