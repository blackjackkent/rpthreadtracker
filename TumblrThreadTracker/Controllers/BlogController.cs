namespace TumblrThreadTracker.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Blogs;

	/// <summary>
	/// Controller class for getting and updating blog information
	/// </summary>
	[RedirectOnMaintenance]
	[Authorize]
	public class BlogController : ApiController
	{
		private readonly IRepository<Blog> _blogRepository;
		private readonly IBlogService _blogService;
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogController"/> class
		/// </summary>
		/// <param name="userBlogRepository">Unity-injected user blog repository</param>
		/// <param name="webSecurityService">Unity-injected web secruty service</param>
		/// <param name="blogService">Unity-injected blog service</param>
		public BlogController(
			IRepository<Blog> userBlogRepository,
			IWebSecurityService webSecurityService,
			IBlogService blogService)
		{
			_blogRepository = userBlogRepository;
			_webSecurityService = webSecurityService;
			_blogService = blogService;
		}

		/// <summary>
		/// Controller endpoint for removing a blog from the database
		/// </summary>
		/// <param name="userBlogId">Unique identifier of blog to be deleted</param>
		public void Delete(int userBlogId)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var blog = _blogService.GetBlogById(userBlogId, _blogRepository);
			if (blog == null || blog.UserId != userId)
			{
				return;
			}
			_blogService.DeleteBlog(blog.UserBlogId.GetValueOrDefault(), _blogRepository);
		}

		/// <summary>
		/// Controller endpoint for getting a specific blog by UserBlogId
		/// </summary>
		/// <param name="id">Unique identifier of blog to be retrieved</param>
		/// <returns><see cref="BlogDto"/> object describing requested blog</returns>
		public BlogDto Get(int id)
		{
			return _blogService.GetBlogById(id, _blogRepository);
		}

		/// <summary>
		/// Controller endpoint for getting all blogs belonging to the currently authenticated user
		/// </summary>
		/// <param name="includeHiatusedBlogs">Whether or not to include blogs the user has marked as on hiatus</param>
		/// <returns>Collection of <see cref="BlogDto"/> objects based on request parameters</returns>
		public IEnumerable<BlogDto> Get(bool includeHiatusedBlogs = false)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository, includeHiatusedBlogs);
			return blogs;
		}

		/// <summary>
		/// Controller endpoint for adding a new blog to the currently authenticated user's account
		/// </summary>
		/// <param name="blogShortname">Shortname of blog to be created</param>
		public void Post([FromBody] string blogShortname)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			if (blogShortname == null || userId == null)
			{
				throw new ArgumentNullException();
			}
			var dto = new BlogDto
				          {
					          UserId = userId.GetValueOrDefault(),
					          BlogShortname = blogShortname,
					          OnHiatus = false
				          };
			_blogService.AddNewBlog(dto, _blogRepository);
		}

		/// <summary>
		/// Controller endpoint for updating an existing blog
		/// </summary>
		/// <param name="request">Request body containing information about blog to be updated</param>
		public void Put(BlogDto request)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			if (request?.UserBlogId == null || userId == null)
			{
				throw new ArgumentNullException();
			}
			_blogService.UpdateBlog(request, _blogRepository);
		}
	}
}