using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TumblrThreadTracker.Infrastructure;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Models.DomainModels.Blogs
{
    public class Blog : DomainModel
    {
        public int? UserBlogId { get; set; }
        public User User { get; set; }
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
    }
}