using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTracker.Models.RequestModels;

namespace TumblrThreadTracker.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IWebSecurityService _webSecurityService;

        public AccountController(IRepository<UserProfile> userProfileRepository, IWebSecurityService webSecurityService)
        {
            _userProfileRepository = userProfileRepository;
            _webSecurityService = webSecurityService;
        }

        public int GetUserId()
        {
            return _webSecurityService.GetUserId(User.Identity.Name);
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post(RegisterRequest request)
        {
            var existingUsername = _userProfileRepository.Get(u => u.UserName == request.Username).Any();
            var existingEmail = _userProfileRepository.Get(u => u.Email == request.Email).Any();

            if (existingUsername || existingEmail)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _webSecurityService.CreateAccount(request.Username, request.Password, request.Email, _userProfileRepository);
            _webSecurityService.Login(request.Username, request.Password);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}