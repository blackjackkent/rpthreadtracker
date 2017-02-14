namespace RPThreadTracker.Infrastructure.Services
{
	using System.Configuration;
	using System.Threading.Tasks;
	using Interfaces;
	using SendGrid;
	using SendGrid.Helpers.Mail;

	/// <inheritdoc cref="IEmailService"/>
	public class EmailService : IEmailService
	{
		/// <inheritdoc cref="IEmailService"/>
		public async Task SendEmail(string recipientAddress, string subject, string body)
		{
			var apiKey = ConfigurationManager.AppSettings["SendGridAPIKey"];
			var sendGridApiClient = new SendGridAPIClient(apiKey);
			var from = new Email(ConfigurationManager.AppSettings["EmailFromAddress"]);
			var to = new Email(recipientAddress);
			var content = new Content("text/html", body);
			var mail = new Mail(from, subject, to, content);
			await sendGridApiClient.client.mail.send.post(requestBody: mail.Get());
		}
	}
}