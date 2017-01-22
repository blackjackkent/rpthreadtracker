using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    [Authorize]
    public class ExportController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Thread> _threadRepository;
        private readonly IWebSecurityService _webSecurityService;
        private readonly IBlogService _blogService;
        private readonly IThreadService _threadService;
        private readonly ITumblrClient _tumblrClient;
	    private readonly IExporterService _exporterService;

	    public ExportController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IWebSecurityService webSecurityService, IBlogService blogService, 
		IThreadService threadService, ITumblrClient tumblrClient, IExporterService exporterService)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
            _webSecurityService = webSecurityService;
            _blogService = blogService;
            _threadService = threadService;
            _tumblrClient = tumblrClient;
		    _exporterService = exporterService;
        }

        public async Task<HttpResponseMessage> Get([FromUri] bool includeArchived = false, [FromUri] bool includeHiatused = false)
        {
	        try
	        {
		        var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity) User.Identity);
		        var blogs =
			        _blogService.GetBlogsByUserId(userId, _blogRepository, includeHiatused).OrderBy(b => b.BlogShortname);
		        var threadDistribution = new Dictionary<int, IEnumerable<ThreadDto>>();
		        var archivedThreadDistribution = new Dictionary<int, IEnumerable<ThreadDto>>();
		        foreach (var blog in blogs)
		        {
			        var threads =
				        _threadService.GetThreadsByBlog(blog, _threadRepository)
					        .OrderBy(t => t.WatchedShortname)
					        .ThenBy(t => t.UserTitle);
			        if (threads.Any())
			        {
				        threadDistribution.Add(blog.UserBlogId.GetValueOrDefault(), threads);
			        }
			        if (includeArchived)
			        {
				        var archivedThreads =
					        _threadService.GetThreadsByBlog(blog, _threadRepository, true)
						        .OrderBy(t => t.WatchedShortname)
						        .ThenBy(t => t.UserTitle);
				        if (archivedThreads.Any())
				        {
					        archivedThreadDistribution.Add(blog.UserBlogId.GetValueOrDefault(), archivedThreads);
				        }
			        }
		        }
		        var package = _exporterService.GetPackage(blogs, threadDistribution, archivedThreadDistribution,
			        includeArchived);
		        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
		        using (MemoryStream ms = new MemoryStream())
		        {
			        byte[] bytes = package.GetAsByteArray();
			        ms.Write(bytes, 0, (int) bytes.Length);
			        MemoryStream copy = new MemoryStream(ms.ToArray());
			        copy.Seek(0, SeekOrigin.Begin);
			        result.Content = new StreamContent(copy);
			        result.Content.Headers.ContentType =
				        new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
			        result.Content.Headers.Add("x-filename", userId + "_Export.xlsx");
			        return result;
		        }
	        }
	        catch (Exception e)
	        {
		        var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
		        result.Content = new StringContent(e.Message);
		        return result;
	        }
        }
    }
}