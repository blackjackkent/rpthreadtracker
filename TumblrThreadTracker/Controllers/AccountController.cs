using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTracker.Models.RequestModels;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    public class AccountController : ApiController
    {
        private readonly IRepository<User> _userProfileRepository;
        private readonly IWebSecurityService _webSecurityService;

        public AccountController(IRepository<User> userProfileRepository, IWebSecurityService webSecurityService)
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
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "An account with some or all of this information already exists.");

            _webSecurityService.CreateAccount(request.Username, request.Password, request.Email, _userProfileRepository);
            _webSecurityService.Login(request.Username, request.Password);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}