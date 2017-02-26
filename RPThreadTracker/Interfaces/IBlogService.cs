namespace RPThreadTracker.Interfaces
{
	using System.Collections.Generic;
	using Infrastructure;
	using Infrastructure.Data;
	using Models.DomainModels.Blogs;

	/// <summary>
	/// Class which facilitates interaction with repository layer
	/// for retrieving <see cref="Blog"/> data
	/// </summary>
	public interface IBlogService
	{
		/// <summary>
		/// Writes new <see cref="Blog"/> object to database
		/// </summary>
		/// <param name="dto"><see cref="BlogDto"/> object containing information about blog to be created.</param>
		/// <param name="blogRepository">Repository object containing database connection</param>
		/// <returns><see cref="BlogDto" /> object returned by insert</returns>
		BlogDto AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository);

		/// <summary>
		/// Removes blog with passed identifier from database
		/// </summary>
		/// <param name="userBlogId">Unique identifier of blog to be deleted</param>
		/// <param name="blogRepository">Repository object containing database connection</param>
		void DeleteBlog(int userBlogId, IRepository<Blog> blogRepository);

		/// <summary>
		/// Retrieves blog with passed identifier from database
		/// </summary>
		/// <param name="userBlogId">Unique identifier of blog to be retrieved</param>
		/// <param name="blogRepository">Repository object containing database connection</param>
		/// <returns><see cref="BlogDto"/> object representing retrieved blog</returns>
		BlogDto GetBlogById(int userBlogId, IRepository<Blog> blogRepository);

		/// <summary>
		/// Retrieves blog with passed shortname from database
		/// </summary>
		/// <param name="blogShortname">Shortname of blog to be retrieved</param>
		/// <param name="userId">Unique identifier of user whose account should be searched</param>
		/// <param name="blogRepository">Repository object containing database connection</param>
		/// <returns><see cref="BlogDto"/> object representing retrieved blog</returns>
		BlogDto GetBlogByShortname(string blogShortname, int userId, IRepository<Blog> blogRepository);

		/// <summary>
		/// Retrieves all blogs belonging to a particular user account
		/// </summary>
		/// <param name="id">Unique identifier of <see cref="UserProfile"/> associated with blogs</param>
		/// <param name="userBlogRepository">Repository object containing database connection</param>
		/// <param name="includeHiatusedBlogs">Whether or not to include blogs user has marked as OnHiatus</param>
		/// <returns>List of blog information associated with user account</returns>
		IEnumerable<BlogDto> GetBlogsByUserId(int? id, IRepository<Blog> userBlogRepository, bool includeHiatusedBlogs);

		/// <summary>
		/// Updates existing blog with passed property information
		/// </summary>
		/// <param name="dto"><see cref="BlogDto"/> object containing data to be updated on database object</param>
		/// <param name="userBlogRepository">Repository object containing database connection</param>
		void UpdateBlog(BlogDto dto, IRepository<Blog> userBlogRepository);

		/// <summary>
		/// Determines whether or not a particular user is the owner of a particular blog
		/// </summary>
		/// <param name="userBlogId">Unique identifier of blog to check</param>
		/// <param name="userId">Unique identifier of user account to check</param>
		/// <param name="userBlogRepository">Repository object containing database connection</param>
		/// <returns>True if user is associated with blog, false if not</returns>
		bool UserOwnsBlog(int userBlogId, int userId, IRepository<Blog> userBlogRepository);

		/// <summary>
		/// Determines whether or not a particular user has a blog tracked with a particular shortname
		/// </summary>
		/// <param name="blogShortname">Shortname string to verify</param>
		/// <param name="userId">Unique identifier of user account to check</param>
		/// <param name="userBlogRepository">Repository object containing database connection</param>
		/// <returns>True if any blog associated with user's account has passed shortname, false if not</returns>
		bool UserIsTrackingShortname(string blogShortname, int? userId, IRepository<Blog> userBlogRepository);
	}
}