﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Services;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Domain.Users
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        {
        }

        public UserProfile(UserProfileDto dto)
        {
            UserId = dto.UserId;
            UserName = dto.UserName;
            Email = dto.Email;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserProfileDto ToDto()
        {
            return new UserProfileDto
            {
                UserId = UserId,
                Email = Email,
                UserName = UserName
            };
        }

        public static UserProfileDto GetByUserId(int id, IUserProfileRepository userProfileRepository)
        {
            var profile = userProfileRepository.GetUserProfileById(id);
            if (profile == null)
                return null;
            return profile.ToDto();
        }

        public static UserProfileDto GetByUsername(string username, IUserProfileRepository userProfileRepository)
        {
            var profile = userProfileRepository.GetUserProfileByUsername(username);
            if (profile == null)
                return null;
            return profile.ToDto();
        }

        public void SendForgotPasswordEmail(string token, IUserProfileRepository userProfileRepository)
        {
            var isValidToken = IsValidToken(token, userProfileRepository);
            if (!isValidToken)
                throw new InvalidDataException();
            var newPassword = ResetPassword(token);
            SendTemporaryPasswordEmail(newPassword);
        }

        private bool IsValidToken(string resetToken, IUserProfileRepository userProfileRepository)
        {
            return userProfileRepository.IsValidPasswordResetToken(UserId, resetToken);
        }

        private string ResetPassword(string resetToken)
        {
            var newPassword = GenerateRandomPassword(6);
            var response = WebSecurity.ResetPassword(resetToken, newPassword);
            if (!response)
                throw new InvalidOperationException();
            return newPassword;
        }

        private void SendTemporaryPasswordEmail(string newPassword)
        {
            const string subject = "RPThreadTracker ~ New Temporary Password";
            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("<p>Hello,</p>");
            bodyBuilder.Append("<p>Below is your autogenerated temporary password for RPThreadTracker:</p>");
            bodyBuilder.Append("<p>" + newPassword + "</p>");
            bodyBuilder.Append(
                "<p>Use this password to log into the tracker; be sure to change your password to something secure once you are logged in.</p>");
            bodyBuilder.Append("<p>Thanks, and have a great day!</p>");
            bodyBuilder.Append("<p>~Tracker-mun</p>");
            var body = bodyBuilder.ToString();
            EmailService.SendEmail(Email, subject, body);
        }

        private static string GenerateRandomPassword(int length)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            var chars = new char[length];
            var rd = new Random();
            for (var i = 0; i < length; i++)
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            return new string(chars);
        }
    }
}