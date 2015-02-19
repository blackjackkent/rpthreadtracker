using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Users;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    public class ForgotPasswordController : ApiController
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<WebpagesMembership> _webpagesMembershipRepository;

        public ForgotPasswordController(IRepository<UserProfile> userProfileRepository,
            IRepository<WebpagesMembership> webpagesMembershipRepository)
        {
            _userProfileRepository = userProfileRepository;
            _webpagesMembershipRepository = webpagesMembershipRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post(string username)
        {
            var user = UserProfile.GetByUsername(username, _userProfileRepository);
            if (user == null)
                throw new ObjectNotFoundException();
            var token = WebSecurity.GeneratePasswordResetToken(username);
            user.ToModel().SendForgotPasswordEmail(token, _webpagesMembershipRepository);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}