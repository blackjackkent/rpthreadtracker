namespace RPThreadTracker.Controllers
{
	using System.Linq;
	using System.Security.Claims;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using Models.RequestModels;

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
		public ExportController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IWebSecurityService webSecurityService, IBlogService blogService, IThreadService threadService, IExporterService exporterService)
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
		public IHttpActionResult Get([FromUri] bool includeArchived = false, [FromUri] bool includeHiatused = false)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository, includeHiatused).OrderBy(b => b.BlogShortname);
			var threadDistribution = _threadService.GetThreadDistribution(blogs, _threadRepository, false);
			var archivedThreadDistribution = _threadService.GetThreadDistribution(blogs, _threadRepository, true);
			var package = _exporterService.GetPackage(blogs, threadDistribution, archivedThreadDistribution, includeArchived);
			var bytes = package.GetAsByteArray();
			return new ExportStreamResult(bytes, this)
			{
				UserId = userId.GetValueOrDefault()
			};
		}
	}
}