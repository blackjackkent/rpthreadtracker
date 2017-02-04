namespace TumblrThreadTracker.Models.DomainModels.Blogs
{
	using Users;

	/// <summary>
	/// Domain Model class representing a user-tracked blog
	/// </summary>
	public class Blog : DomainModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Blog"/> class
		/// </summary>
		public Blog()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Blog"/> class
		/// </summary>
		/// <param name="dto"><see cref="BlogDto"/> object to convert to domain model</param>
		public Blog(BlogDto dto)
		{
			UserBlogId = dto.UserBlogId;
			BlogShortname = dto.BlogShortname;
			UserId = dto.UserId;
			OnHiatus = dto.OnHiatus;
		}

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
		/// Gets or sets user profile object associated with blog
		/// </summary>
		/// <value>
		/// <see cref="User"/> object representing related user profile
		/// </value>
		public User User { get; set; }

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

		/// <summary>
		/// Converts <see cref="Blog"/> object to <see cref="BlogDto"/>
		/// </summary>
		/// <returns><see cref="BlogDto"/> object corresponding to this blog</returns>
		public BlogDto ToDto()
		{
			return new BlogDto
				       {
					       BlogShortname = BlogShortname,
					       UserBlogId = UserBlogId,
					       UserId = UserId,
					       OnHiatus = OnHiatus
				       };
		}
	}
}