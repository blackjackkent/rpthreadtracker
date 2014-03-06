using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.DataModels
{
    [Table("UserBlog")]
    public class UserBlog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserBlogId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserProfile UserProfile { get; set; } 
        public string BlogShortname { get; set; }

        public static UserBlog Create(string blogShortName, int userId)
        {
            return new UserBlog
            {
                BlogShortname = blogShortName,
                UserId = userId
            };
        }
    }
}