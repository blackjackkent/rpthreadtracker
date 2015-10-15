using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Models.DomainModels.Account
{
    public class WebpagesMembershipDto : IDto<WebpagesMembership>
    {
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public WebpagesMembership ToModel()
        {
            return new WebpagesMembership
            {
                UserId = UserId,
                ConfirmationToken = ConfirmationToken,
                CreateDate = CreateDate,
                IsConfirmed = IsConfirmed,
                LastPasswordFailureDate = LastPasswordFailureDate,
                Password = Password,
                PasswordChangedDate = PasswordChangedDate,
                PasswordFailuresSinceLastSuccess = PasswordFailuresSinceLastSuccess,
                PasswordSalt = PasswordSalt,
                PasswordVerificationToken = PasswordVerificationToken,
                PasswordVerificationTokenExpirationDate = PasswordVerificationTokenExpirationDate
            };
        }
    }
}