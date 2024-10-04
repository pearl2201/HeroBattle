using MasterServer.Application.Common.Models;

namespace MasterServer.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
