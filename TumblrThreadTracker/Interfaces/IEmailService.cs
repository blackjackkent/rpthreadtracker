using System.Threading.Tasks;

namespace TumblrThreadTracker.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string newTemporaryPassword, string body);
    }
}