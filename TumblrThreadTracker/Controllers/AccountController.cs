using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.RequestModels;
using TumblrThreadTracker.Repositories;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public AccountController(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public int GetUserId()
        {
            return WebSecurity.GetUserId(User.Identity.Name);
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Post(RegisterRequest request)
        {
            var existingUsername = _userProfileRepository.Get(u => u.UserName == request.Username);
            var existingEmail = _userProfileRepository.Get(u => u.Email == request.Email);

            if (existingUsername != null || existingEmail != null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            WebSecurity.CreateUserAndAccount(request.Username, request.Password);
            WebSecurity.Login(request.Username, request.Password);
            var profile = new UserProfile
            {
                UserId = WebSecurity.GetUserId(request.Username),
                UserName = request.Username,
                Email = request.Email
            };
            _userProfileRepository.Update(profile);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
