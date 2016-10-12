using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string emailid, string subject, string body)
        {
            var apiKey = ConfigurationManager.AppSettings["SendGridAPIKey"];
            var sendGridApiClient = new SendGridAPIClient(apiKey);

            var from = new Email("noreply@rpthreadtracker.com");
            var to = new Email(emailid);
            var content = new Content("text/html", body);
            var mail = new Mail(from, subject, to, content);

            await sendGridApiClient.client.mail.send.post(requestBody: mail.Get());
        }
    }
}