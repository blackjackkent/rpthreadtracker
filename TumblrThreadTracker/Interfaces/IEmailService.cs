namespace TumblrThreadTracker.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string email, string newTemporaryPassword, string body);
    }
}