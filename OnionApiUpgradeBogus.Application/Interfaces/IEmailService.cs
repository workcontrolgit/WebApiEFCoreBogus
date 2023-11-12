using OnionApiUpgradeBogus.Application.DTOs.Email;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}