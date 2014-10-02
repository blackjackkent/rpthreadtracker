using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    public class ForgotPasswordController : ApiController
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public ForgotPasswordController()
        {
            _userProfileRepository = new UserProfileRepository(new ThreadTrackerContext());
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post(string username)
        {
            //check user existance
            UserProfileDto user = UserProfile.GetByUsername(username, _userProfileRepository);
            if (user == null)
                throw new ObjectNotFoundException();
            //generate password token
            string token = WebSecurity.GeneratePasswordResetToken(username);
            user.ToModel().SendForgotPasswordEmail(token, _userProfileRepository);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}