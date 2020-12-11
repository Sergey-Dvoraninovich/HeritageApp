using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HeritageWebApplication.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}