namespace RPThreadTracker.Interfaces
{
	using System.Collections.Generic;
	using Infrastructure;
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
		void AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository);

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
	}
}