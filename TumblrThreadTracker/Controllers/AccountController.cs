using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTracker.Models.RequestModels;

namespace TumblrThreadTracker.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IRepository<User> _userProfileRepository;
        private readonly IWebSecurityService _webSecurityService;

        public AccountController(IRepository<User> userProfileRepository, IWebSecurityService webSecurityService)
        {
            _userProfileRepository = userProfileRepository;
            _webSecurityService = webSecurityService;
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post(RegisterRequest request)
        {
            //@TODO fix front-end behavior to clarify between bad request and error
            var existingUsername = _userProfileRepository.Get(u => u.UserName == request.Username).Any();
            var existingEmail = _userProfileRepository.Get(u => u.Email == request.Email).Any();

            if (existingUsername || existingEmail)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _webSecurityService.CreateAccount(request.Username, request.Password, request.Email, _userProfileRepository);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}