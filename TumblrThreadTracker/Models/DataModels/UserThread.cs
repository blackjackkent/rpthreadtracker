using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.DataModels
{
    [Table("UserThread")]
    public class UserThread
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserThreadId { get; set; }
        public int UserBlogId { get; set; }
        [ForeignKey("UserBlogId")]
        public UserBlog UserBlog { get; set; }
        public string PostId { get; set; }
        public string UserTitle { get; set; }
    }
}