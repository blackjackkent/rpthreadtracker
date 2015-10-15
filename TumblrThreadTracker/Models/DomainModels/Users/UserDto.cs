using System;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Models.DomainModels.Users
{
    public class UserDto : IDto<User>
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }

        public User ToModel()
        {
            return new User(this);
        }
    }
}