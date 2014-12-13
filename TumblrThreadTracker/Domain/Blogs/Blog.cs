﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Domain.Blogs
{
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
        public static BlogDto GetBlogById(int userBlogId, IUserBlogRepository blogRepository)
        {
            Blog blog = blogRepository.GetUserBlogById(userBlogId);
            return blog.ToDto();
        }

        [ExcludeFromCodeCoverage]
        public static IEnumerable<BlogDto> GetBlogsByUserId(int? id, IUserBlogRepository userBlogRepository)
        {
            if (id == null)
                return null;
            var blogList = new List<BlogDto>();
            IEnumerable<Blog> blogs = userBlogRepository.GetUserBlogs(id);
            foreach (Blog blog in blogs)
                blogList.Add(blog.ToDto());
            return blogList;
        }

        [ExcludeFromCodeCoverage]
        public static BlogDto GetBlogByShortname(string shortname, int userId, IUserBlogRepository userBlogRepository)
        {
            if (shortname == null)
                return null;
            Blog blog = userBlogRepository.GetUserBlogByShortname(shortname, userId);
            return blog.ToDto();
        }

        [ExcludeFromCodeCoverage]
        public static void AddNewBlog(BlogDto dto, IUserBlogRepository blogRepository)
        {
            blogRepository.InsertUserBlog(new Blog(dto));
        }

        [ExcludeFromCodeCoverage]
        public static void UpdateBlog(BlogDto dto, IUserBlogRepository blogRepository)
        {
            blogRepository.UpdateUserBlog(new Blog(dto));
        }

        public static void DeleteBlog(BlogDto blog, IUserBlogRepository blogRepository)
        {
            blogRepository.DeleteUserBlog(blog.UserBlogId);
        }
    }
}