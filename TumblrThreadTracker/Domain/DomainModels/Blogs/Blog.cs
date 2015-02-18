using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Domain.Blogs
{
    using System.Linq;

    [Table("UserBlog")]
    public class Blog
    {
        [ExcludeFromCodeCoverage]
        public Blog()
        {
        }

        public Blog(BlogDto dto)
        {
            UserBlogId = dto.UserBlogId;
            BlogShortname = dto.BlogShortname;
            UserId = dto.UserId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserBlogId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile UserProfile { get; set; }

        public string BlogShortname { get; set; }

        public BlogDto ToDto()
        {
            return new BlogDto
            {
                BlogShortname = BlogShortname,
                UserBlogId = UserBlogId,
                UserId = UserId
            };
        }

        [ExcludeFromCodeCoverage]
        public static BlogDto GetBlogById(int userBlogId, IRepository<Blog> blogRepository)
        {
            Blog blog = blogRepository.Get(userBlogId);
            return blog.ToDto();
        }

        [ExcludeFromCodeCoverage]
        public static IEnumerable<BlogDto> GetBlogsByUserId(int? id, IRepository<Blog> userBlogRepository)
        {
            if (id == null)
                return null;
            var blogList = new List<BlogDto>();
            IEnumerable<Blog> blogs = userBlogRepository.Get(b => b.UserId == id);
            foreach (Blog blog in blogs)
                blogList.Add(blog.ToDto());
            return blogList;
        }

        [ExcludeFromCodeCoverage]
        public static BlogDto GetBlogByShortname(string shortname, int userId, IRepository<Blog> userBlogRepository)
        {
            if (shortname == null)
                return null;
            Blog blog = userBlogRepository.Get(b => b.BlogShortname == shortname && b.UserId == userId).FirstOrDefault();
            return blog.ToDto();
        }

        [ExcludeFromCodeCoverage]
        public static void AddNewBlog(BlogDto dto, IRepository<Blog> blogRepository)
        {
            blogRepository.Insert(new Blog(dto));
        }

        [ExcludeFromCodeCoverage]
        public static void UpdateBlog(BlogDto dto, IRepository<Blog> blogRepository)
        {
            blogRepository.Update(new Blog(dto));
        }

        public static void DeleteBlog(BlogDto blog, IRepository<Blog> blogRepository)
        {
            blogRepository.Delete(blog.UserBlogId);
        }
    }
}