namespace TumblrThreadTracker.Interfaces
{
	using System.Threading.Tasks;

	/// <summary>
	/// Class responsible for sending emails to users via SendGrid API
	/// </summary>
	public interface IEmailService
	{
		/// <summary>
		/// Sends email via sendgrid API to passed recipient
		/// </summary>
		/// <param name="recipientAddress">Address to send the email to</param>
		/// <param name="subject">Subject line of email</param>
		/// <param name="body">HTML string representing email body</param>
		/// <returns>Dynamic object returned from SendGrid API</returns>
		Task SendEmail(string recipientAddress, string subject, string body);
	}
}