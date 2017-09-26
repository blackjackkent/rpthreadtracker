namespace RPThreadTracker.Infrastructure.Services
{
	using System;
	using System.Collections.Generic;
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

		/// <inheritdoc />
		public async Task SendPasswordResetEmail(string recipientAddress, string username, string newPassword, IConfigurationService configurationService)
		{
			var apiKey = configurationService.SendGridApiKey;
			var sendGridApiClient = new SendGridAPIClient(apiKey);
			var message = new Mail()
			{
				From = new Email(configurationService.EmailFromAddress),
				Subject = "RPThreadTracker Password Reset",
				TemplateId = configurationService.PasswordResetEmailTemplateId
			};
			try
			{
				message.Personalization = new List<Personalization>
				{
					new Personalization
					{
						Tos = new List<Email>
						{
							new Email(recipientAddress)
						},
						Substitutions = new Dictionary<string, string>
						{
							{ "<%newPassword%>", newPassword },
							{ "<%username%>", username },
						}
					}
				};
			}
			catch (Exception e)
			{
				var ex = e;
			}
			await sendGridApiClient.client.mail.send.post(requestBody: message.Get());
		}
	}
}