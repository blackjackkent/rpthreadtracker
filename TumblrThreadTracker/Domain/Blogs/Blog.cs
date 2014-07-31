using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Domain.Blogs
{
    [Table("UserBlog")]
    public class Blog
    {
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
        public int UserBlogId { get; set; }
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

        public static IEnumerable<BlogDto> GetBlogsByUserId(int? id, IUserBlogRepository userBlogRepository)
        {
            if (id == null)
            {
                return null;
            }
            var blogList = new List<BlogDto>();
            IEnumerable<Blog> blogs = userBlogRepository.GetUserBlogs(id);
            foreach (Blog blog in blogs)
            {
                blogList.Add(blog.ToDto());
            }
            return blogList;
        }
    }
}