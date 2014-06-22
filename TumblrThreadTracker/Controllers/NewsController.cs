using System;
using System.Web.Http;
using TumblrThreadTracker.Models.ViewModels;
using TumblrThreadTracker.Services;

namespace TumblrThreadTracker.Controllers
{
    public class NewsController : ApiController
    {
        // GET api/<controller>
        public Thread Get()
        {
            Thread thread = ThreadService.GetNewsThread();
            return thread;
        }

        // POST api/<controller>
        public void Post()
        {
            throw new NotImplementedException();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}