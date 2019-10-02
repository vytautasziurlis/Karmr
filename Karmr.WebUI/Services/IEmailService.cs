using System.Threading.Tasks;

namespace Karmr.WebUI.Services
{
    public interface IEmailService
    {
        Task SendPasswordReset(string to, string resetUrl);

        Task SendPasswordChangeNotification(string to);
    }
}