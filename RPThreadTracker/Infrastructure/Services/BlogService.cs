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
		public void AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository)
		{
			blogRepository.Insert(new Blog(dto));
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
			var userOwnsBlog = userBlogRepository.Get(t => t.UserBlogId == userBlogId && t.UserId == userId).FirstOrDefault();
			return userOwnsBlog != null;
		}
	}
}