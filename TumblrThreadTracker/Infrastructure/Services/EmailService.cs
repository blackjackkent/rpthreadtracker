using System.Net;
using System.Net.Mail;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string emailid, string subject, string body)
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
            var msg = new MailMessage
            {
                From = new MailAddress("postmaster@rpthreadtracker.com")
            };
            msg.To.Add(new MailAddress(emailid));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;
            client.Send(msg);
        }
    }
}