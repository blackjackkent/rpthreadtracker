namespace RPThreadTracker.Infrastructure.Services
{
	using System.Threading.Tasks;
	using Filters;
	using Interfaces;
	using SendGrid;
	using SendGrid.Helpers.Mail;

	/// <inheritdoc cref="IEmailClient"/>
	public class SendGridEmailClient : IEmailClient
	{
		/// <inheritdoc cref="IEmailClient"/>
		[ExcludeFromCoverage]
		public async Task SendEmail(string recipientAddress, string subject, string body, IConfigurationService configurationService)
		{
			var apiKey = configurationService.SendGridApiKey;
			var sendGridApiClient = new SendGridAPIClient(apiKey);
			var from = new Email(configurationService.EmailFromAddress);
			var to = new Email(recipientAddress);
			var content = new Content("text/html", body);
			var mail = new Mail(from, subject, to, content);
			await sendGridApiClient.client.mail.send.post(requestBody: mail.Get());
		}
	}
}