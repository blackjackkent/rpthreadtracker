namespace TumblrThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;

	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Blogs;

	public class UserBlogRepository : BaseRepository<Blog, UserBlog>
	{
		private readonly IThreadTrackerContext _context;

		private readonly IDbSet<UserBlog> _dbSet;

		public UserBlogRepository(IThreadTrackerContext context)
		{
			_context = context;
			_dbSet = context.UserBlogs;
		}

		protected override IThreadTrackerContext Context
		{
			get
			{
				return _context;
			}
		}

		protected override IDbSet<UserBlog> DbSet
		{
			get
			{
				return _dbSet;
			}
		}
	}
}