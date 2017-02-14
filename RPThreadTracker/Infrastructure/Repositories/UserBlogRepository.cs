namespace RPThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;
	using Interfaces;
	using Models.DomainModels.Blogs;

	/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
	public class UserBlogRepository : BaseRepository<Blog, UserBlog>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserBlogRepository"/> class
		/// </summary>
		/// <param name="context">Entity framework data context to use for tracker data management</param>
		public UserBlogRepository(IThreadTrackerContext context)
		{
			Context = context;
			DbSet = context.UserBlogs;
		}

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IThreadTrackerContext Context { get; }

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IDbSet<UserBlog> DbSet { get; }
	}
}