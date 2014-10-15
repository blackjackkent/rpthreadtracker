using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TumblrThreadTracker.Services
{
    public class EmailService
    {
        public static void SendEmail(string emailid, string subject, string body)
        {
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = "mail.rpthreadtracker.com",
                Port = 25,
                UseDefaultCredentials = false
            };

            var credentials = new NetworkCredential("postmaster@rpthreadtracker.com", "***REMOVED***");
            client.Credentials = credentials;

            var msg = new MailMessage();
            msg.From = new MailAddress("postmaster@rpthreadtracker.com");
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }
    }
}