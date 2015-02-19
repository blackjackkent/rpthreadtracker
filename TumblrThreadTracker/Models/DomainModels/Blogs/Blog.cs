using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Models.DomainModels.Blogs
{
    [Table("UserBlog")]
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserBlogId { get; set; }
        [ForeignKey("UserId")]
        public UserProfile UserProfile { get; set; }
        public int UserId { get; set; }
        public string BlogShortname { get; set; }

        public Blog()
        {
        }

        public Blog(BlogDto dto)
        {
            UserBlogId = dto.UserBlogId;
            BlogShortname = dto.BlogShortname;
            UserId = dto.UserId;
        }

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
            var blog = blogRepository.Get(userBlogId);
            return blog.ToDto();
        }

        [ExcludeFromCodeCoverage]
        public static IEnumerable<BlogDto> GetBlogsByUserId(int? id, IRepository<Blog> userBlogRepository)
        {
            if (id == null)
                return null;
            var blogs = userBlogRepository.Get(b => b.UserId == id);
            return blogs.Select(blog => blog.ToDto()).ToList();
        }

        [ExcludeFromCodeCoverage]
        public static BlogDto GetBlogByShortname(string shortname, int userId, IRepository<Blog> userBlogRepository)
        {
            if (shortname == null)
                return null;
            var blog = userBlogRepository.Get(b => b.BlogShortname == shortname && b.UserId == userId).FirstOrDefault();
            return blog != null ? blog.ToDto() : null;
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