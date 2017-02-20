namespace RPThreadTracker.Infrastructure.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using Interfaces;
	using Models.DomainModels.Blogs;

	/// <inheritdoc cref="IBlogService"/>
	public class BlogService : IBlogService
	{
		/// <inheritdoc cref="IBlogService"/>
		public BlogDto AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository)
		{
			var blog = blogRepository.Insert(new Blog(dto));
			return blog.ToDto();
		}

		/// <inheritdoc cref="IBlogService"/>
		public void DeleteBlog(int userBlogId, IRepository<Blog> blogRepository)
		{
			blogRepository.Delete(userBlogId);
		}

		/// <inheritdoc cref="IBlogService"/>
		public BlogDto GetBlogById(int userBlogId, IRepository<Blog> blogRepository)
		{
			var blog = blogRepository.GetSingle(b => b.UserBlogId == userBlogId);
			return blog.ToDto();
		}

		/// <inheritdoc cref="IBlogService"/>
		public IEnumerable<BlogDto> GetBlogsByUserId(int? id, IRepository<Blog> userBlogRepository, bool includeHiatusedBlogs)
		{
			if (id == null)
			{
				return null;
			}
			var blogs = userBlogRepository.Get(b => b.UserId == id);
			if (!includeHiatusedBlogs)
			{
				blogs = blogs.Where(b => !b.OnHiatus);
			}
			return blogs.Select(blog => blog.ToDto()).ToList();
		}

		/// <inheritdoc cref="IBlogService"/>
		public void UpdateBlog(BlogDto dto, IRepository<Blog> userBlogRepository)
		{
			userBlogRepository.Update(dto.UserBlogId, new Blog(dto));
		}

		/// <inheritdoc cref="IBlogService"/>
		public bool UserOwnsBlog(int userBlogId, int userId, IRepository<Blog> userBlogRepository)
		{
			var userOwnsBlog = userBlogRepository.Get(b => b.UserBlogId == userBlogId && b.UserId == userId).FirstOrDefault();
			return userOwnsBlog != null;
		}

		/// <inheritdoc cref="IBlogService"/>
		public bool UserIsTrackingShortname(string blogShortname, int? userId, IRepository<Blog> userBlogRepository)
		{
			var userIsTrackingShortname = userBlogRepository.Get(b => b.UserId == userId && b.BlogShortname == blogShortname).FirstOrDefault();
			return userIsTrackingShortname != null;
		}
	}
}