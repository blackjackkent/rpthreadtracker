using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Models.DomainModels.Blogs
{
    [Table("UserBlog")]
    public class Blog : DomainModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserBlogId { get; set; }
        [ForeignKey("UserId")]
        public UserProfile UserProfile { get; set; }
        public int UserId { get; set; }
        public string BlogShortname { get; set; }

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
    }
}