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
    using Models.DataModels;

    public class ForgotPasswordController : ApiController
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<webpages_Membership> _webpages_MembershipRepository; 

        public ForgotPasswordController(IRepository<UserProfile> userProfileRepository, IRepository<webpages_Membership> webpages_MembershipRepository)
        {
            _userProfileRepository = userProfileRepository;
            _webpages_MembershipRepository = webpages_MembershipRepository;
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
            user.ToModel().SendForgotPasswordEmail(token, _webpages_MembershipRepository);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}