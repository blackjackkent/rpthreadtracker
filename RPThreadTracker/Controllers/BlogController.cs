namespace RPThreadTracker.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Net;
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
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="blogService">Unity-injected blog service</param>
		public BlogController(IRepository<Blog> userBlogRepository, IWebSecurityService webSecurityService, IBlogService blogService)
		{
			_blogRepository = userBlogRepository;
			_webSecurityService = webSecurityService;
			_blogService = blogService;
		}

		/// <summary>
		/// Controller endpoint for getting a specific blog by UserBlogId
		/// </summary>
		/// <param name="id">Unique identifier of blog to be retrieved</param>
		/// <returns><see cref="BlogDto"/> object describing requested blog</returns>
		public IHttpActionResult Get(int id)
		{
			var blog = _blogService.GetBlogById(id, _blogRepository);
			if (blog == null)
			{
				return NotFound();
			}
			return Ok(blog);
		}

		/// <summary>
		/// Controller endpoint for getting all blogs belonging to the currently authenticated user
		/// </summary>
		/// <param name="includeHiatusedBlogs">Whether or not to include blogs the user has marked as on hiatus</param>
		/// <returns>ActionResult object wrapping collection of <see cref="BlogDto"/> objects</returns>
		public IHttpActionResult Get(bool includeHiatusedBlogs = false)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository, includeHiatusedBlogs);
			return Ok(blogs);
		}

		/// <summary>
		/// Controller endpoint for adding a new blog to the currently authenticated user's account
		/// </summary>
		/// <param name="blogShortname">Shortname of blog to be created</param>
		/// <returns>ActionResult object wrapping HTTP response</returns>
		public IHttpActionResult Post([FromBody] string blogShortname)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			if (blogShortname == null || userId == null)
			{
				return BadRequest();
			}
			var blogExists = _blogService.UserIsTrackingShortname(blogShortname, userId, _blogRepository);
			if (blogExists)
			{
				return BadRequest();
			}
			var dto = new BlogDto
			{
				UserId = userId.GetValueOrDefault(),
				BlogShortname = blogShortname,
				OnHiatus = false
			};
			var createdBlog = _blogService.AddNewBlog(dto, _blogRepository);
			return CreatedAtRoute("DefaultApi", new { id = createdBlog.UserBlogId }, createdBlog);
		}

		/// <summary>
		/// Controller endpoint for updating an existing blog
		/// </summary>
		/// <param name="request">Request body containing information about blog to be updated</param>
		/// <returns>ActionResult object wrapping HTTP response</returns>
		public IHttpActionResult Put(BlogDto request)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			if (request?.UserBlogId == null || userId == null)
			{
				return BadRequest();
			}
			if (!_blogService.UserOwnsBlog(request.UserBlogId.GetValueOrDefault(), userId.GetValueOrDefault(), _blogRepository))
			{
				return BadRequest();
			}
			_blogService.UpdateBlog(request, _blogRepository);
			return Ok();
		}

		/// <summary>
		/// Controller endpoint for removing a blog from the database
		/// </summary>
		/// <param name="userBlogId">Unique identifier of blog to be deleted</param>
		/// <returns>ActionResult object wrapping HTTP response</returns>
		public IHttpActionResult Delete(int userBlogId)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var blog = _blogService.GetBlogById(userBlogId, _blogRepository);
			if (blog == null || blog.UserId != userId)
			{
				return BadRequest();
			}
			_blogService.DeleteBlog(blog.UserBlogId.GetValueOrDefault(), _blogRepository);
			return Ok();
		}
	}
}