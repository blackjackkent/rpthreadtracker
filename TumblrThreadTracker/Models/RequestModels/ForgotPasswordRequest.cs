using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.RequestModels
{
    public class ForgotPasswordRequest
    {
        public string UsernameOrEmail { get; set; }
    }
}