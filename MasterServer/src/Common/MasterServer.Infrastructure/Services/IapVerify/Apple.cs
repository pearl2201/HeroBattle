using Iap.Verify.Models;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Iap.Verify
{
    public abstract class Apple : IIapVerify
    {
        protected const string Production = nameof(Production);
        protected const string GraceDays = nameof(GraceDays);

        private readonly IApplicationDbContext _dbContext;

        public Apple(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected ServiceResult<ValidationIapResult> LogVerificationResultAsync(long playerId, string validatorName, Receipt receipt, ValidationIapResult result, ILogger log, CancellationToken cancellationToken)
        {

            if (result.IsValid && result.ValidatedReceipt is not null)
            {
                log.LogInformation("Validated IAP '{BundleId}':'{ProductId}'", receipt.BundleId, receipt.ProductId);
                return ServiceResult.Success(result);
            }

            if (!string.IsNullOrEmpty(receipt?.BundleId) &&
                !string.IsNullOrEmpty(receipt?.ProductId))
            {
                log.LogInformation("Failed to validate IAP '{BundleId}':'{ProductId}', reason '{Message}'", receipt.BundleId, receipt.ProductId, result?.Message ?? string.Empty);
            }
            else
            {
                log.LogInformation("Failed to validate IAP, reason '{Message}'", result?.Message ?? string.Empty);
            }

            return ServiceResult.Failed<ValidationIapResult>(ServiceError.Validation); ;
        }

        public abstract Task<ServiceResult<ValidationIapResult>> Run(long playerId, Receipt receipt, CancellationToken cancellationToken);
    }
}
