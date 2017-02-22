namespace RPThreadTracker.Interfaces
{
	using System.Threading.Tasks;

	/// <summary>
	/// Wrapper class for client used to send emails from tracker
	/// </summary>
	public interface IEmailClient
	{
		/// <summary>
		/// Sends email via sendgrid API to passed recipient
		/// </summary>
		/// <param name="recipientAddress">Address to send the email to</param>
		/// <param name="subject">Subject line of email</param>
		/// <param name="body">HTML string representing email body</param>
		/// <param name="configurationService">Wrapper service for application config information</param>
		/// <returns>Task for async processing</returns>
		Task SendEmail(string recipientAddress, string subject, string body, IConfigurationService configurationService);
	}
}