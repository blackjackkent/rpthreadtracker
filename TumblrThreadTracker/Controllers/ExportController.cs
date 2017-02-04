namespace TumblrThreadTracker.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using System.Web.Http;

	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;

	/// <summary>
	/// Controller class for exporting thread data
	/// </summary>
	[RedirectOnMaintenance]
	[Authorize]
	public class ExportController : ApiController
	{
		private readonly IRepository<Blog> _blogRepository;

		private readonly IBlogService _blogService;

		private readonly IExporterService _exporterService;

		private readonly IRepository<Thread> _threadRepository;

		private readonly IThreadService _threadService;

		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportController"/> class
		/// </summary>
		/// <param name="userBlogRepository">Unity-injected user blog repository</param>
		/// <param name="userThreadRepository">Unity-injected user thread repository</param>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="blogService">Unity-injected blog service</param>
		/// <param name="threadService">Unity-injected thread service</param>
		/// <param name="exporterService">Unity-injected exporter service</param>
		public ExportController(
			IRepository<Blog> userBlogRepository,
			IRepository<Thread> userThreadRepository,
			IWebSecurityService webSecurityService,
			IBlogService blogService,
			IThreadService threadService,
			IExporterService exporterService)
		{
			_blogRepository = userBlogRepository;
			_threadRepository = userThreadRepository;
			_webSecurityService = webSecurityService;
			_blogService = blogService;
			_threadService = threadService;
			_exporterService = exporterService;
		}

		/// <summary>
		/// Retrieves a bytestream representing an Excel file with exported thread data
		/// </summary>
		/// <param name="includeArchived">Whether or not to include threads marked as Archived</param>
		/// <param name="includeHiatused">Whether or not to include blogs marked as OnHiatus</param>
		/// <returns>HttpResponseMessage containing thread data exported to excel file (or BadRequest if export failed)</returns>
		public async Task<HttpResponseMessage> Get(
			[FromUri] bool includeArchived = false,
			[FromUri] bool includeHiatused = false)
		{
			try
			{
				var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
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

				var package = _exporterService.GetPackage(
					blogs,
					threadDistribution,
					archivedThreadDistribution,
					includeArchived);
				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				using (MemoryStream ms = new MemoryStream())
				{
					byte[] bytes = package.GetAsByteArray();
					ms.Write(bytes, 0, bytes.Length);
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