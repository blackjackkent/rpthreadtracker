using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Services;

namespace TumblrThreadTracker.Controllers
{
    public class NewsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Thread> Get()
        {
            /*IEnumerable<Thread> threads = ThreadService.GetNewsThreads(5);
            return threads;*/
            throw new NotImplementedException();
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