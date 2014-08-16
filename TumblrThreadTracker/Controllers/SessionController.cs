using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Models;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    public class SessionController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(LoginModel model)
        {
            if (ModelState == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            return WebSecurity.Login(model.UserName, model.Password, model.RememberMe)
                ? new HttpResponseMessage(HttpStatusCode.OK)
                : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [HttpDelete]
        public void Delete()
        {
            WebSecurity.Logout();
        }
    }
}