namespace RPThreadTracker.Models.DomainModels.Blogs
{
	using Interfaces;

	/// <summary>
	/// DTO object for transferring <see cref="Blog" /> data
	/// </summary>
	public class BlogDto : IDto<Blog>
	{
		/// <summary>
		/// Gets or sets shortname (username) value of Tumblr blog being tracked
		/// </summary>
		/// <value>
		/// String value of shortname
		/// </value>
		public string BlogShortname { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not the user has marked the blog as hiatused
		/// </summary>
		/// <value>
		/// True if blog hiatused, otherwise false
		/// </value>
		public bool OnHiatus { get; set; }

		/// <summary>
		/// Gets or sets unique identifier for blog object
		/// </summary>
		/// <value>
		/// Integer value of user blog, or null if blog is not yet in database
		/// </value>
		public int? UserBlogId { get; set; }

		/// <summary>
		/// Gets or sets unique identifier of user profile associated with blog
		/// </summary>
		/// <value>
		/// Integer value of user profile ID
		/// </value>
		public int UserId { get; set; }

		/// <inheritdoc cref="IDto{TModel}"/>
		public Blog ToModel()
		{
			return new Blog(this);
		}
	}
}