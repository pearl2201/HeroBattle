using Iap.Verify.Models;
using MasterServer.Application.Common.Models;

namespace MasterServer.Application.Common.Interfaces
{
    public interface IIapVerify
    {
        Task<ServiceResult<ValidationIapResult>> Run(long playerId, Receipt receipt, CancellationToken cancellationToken);
    }
}
