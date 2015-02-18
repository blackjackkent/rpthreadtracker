using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.RequestModels
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}