//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RPThreadTracker.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.UserBlogs = new HashSet<UserBlog>();
        }
    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public bool ShowDashboardThreadDistribution { get; set; }
        public bool UseInvertedTheme { get; set; }
    
        public virtual ICollection<UserBlog> UserBlogs { get; set; }
    }
}