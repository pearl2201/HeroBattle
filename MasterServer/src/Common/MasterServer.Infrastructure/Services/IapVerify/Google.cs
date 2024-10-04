using Google.Apis.AndroidPublisher.v3;
using Google.Apis.AndroidPublisher.v3.Data;
using Iap.Verify.Models;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Iap.Verify
{
    public class GoogleVerify : IIapVerify
    {
        private const string ValidatorRoute = "v1/Google";
        private const string GraceDays = nameof(GraceDays);

        // https://developers.google.com/android-publisher/api-ref/rest
        private readonly AndroidPublisherService _googleService;

        private readonly IApplicationDbContext _dbContext;

        private readonly ILogger _logger;

        private readonly int _graceDays;

        public GoogleVerify(
            AndroidPublisherService googleService,
            IApplicationDbContext dbContext,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _googleService = googleService;
            _dbContext = dbContext;

            _logger = loggerFactory.CreateLogger<GoogleVerify>();

            _ = int.TryParse(configuration[GraceDays], out _graceDays);
        }


        public async Task<ServiceResult<ValidationIapResult>> Run(long playerId, Receipt receipt, CancellationToken cancellationToken)
        {
            var result = default(ValidationIapResult);

            if (receipt?.IsValid() == true)
            {
                var iapTask = GetInAppProductAsync(receipt.BundleId, receipt.ProductId, cancellationToken);
                var subTask = GetSubscriptionAsync(receipt.BundleId, receipt.ProductId, cancellationToken);

                if (await iapTask is not null)
                {
                    // Support legacy subscriptions
                    result = string.Equals(iapTask.Result.PurchaseType, "subscription", StringComparison.OrdinalIgnoreCase)
                        ? await ValidateSubscriptionAsync(receipt, _logger, cancellationToken)
                        : await ValidateProductAsync(receipt, _logger, cancellationToken);
                }
                else if (await subTask is not null)
                {
                    result = await ValidateSubscriptionAsync(receipt, _logger, cancellationToken);
                }
                else
                {
                    result = new ValidationIapResult(false, $"IAP '{receipt.BundleId}':'{receipt.ProductId}' not found");
                }
            }
            else
            {
                result = new ValidationIapResult(false, $"Invalid {nameof(Receipt)}");
            }

            if (result.IsValid && result.ValidatedReceipt is not null)
            {
                _logger.LogInformation("Validated IAP '{BundleId}':'{ProductId}'", receipt.BundleId, receipt.ProductId);
                return ServiceResult.Success<ValidationIapResult>(result);
            }

            if (!string.IsNullOrEmpty(receipt?.BundleId) &&
                !string.IsNullOrEmpty(receipt?.ProductId))
            {
                _logger.LogInformation("Failed to validate IAP '{BundleId}':'{ProductId}', reason '{Message}'", receipt.BundleId, receipt.ProductId, result?.Message ?? string.Empty);
            }
            else
            {
                _logger.LogInformation("Failed to validate IAP, reason '{Message}'", result?.Message ?? string.Empty);
            }

            return ServiceResult.Failed<ValidationIapResult>(ServiceError.Validation);
        }

        private async Task<ValidationIapResult> ValidateProductAsync(Receipt receipt, ILogger log, CancellationToken cancellationToken)
        {
            var result = default(ValidationIapResult);

            try
            {
                var request = _googleService.Purchases.Products.Get(receipt.BundleId, receipt.ProductId, receipt.Token);
                var purchase = await request.ExecuteAsync(cancellationToken);

                receipt.Environment = purchase is null
                    ? EnvironmentType.Unknown
                    : purchase.PurchaseType == 0 ? EnvironmentType.Test : EnvironmentType.Production;

                if (purchase is null)
                {
                    result = new ValidationIapResult(false, $"no purchase found");
                }
                else if (!purchase.OrderId.StartsWith(receipt.TransactionId, StringComparison.Ordinal))
                {
                    result = new ValidationIapResult(false, $"transaction id '{receipt.TransactionId}' does not match '{purchase.OrderId}'");
                }
                else if (purchase.PurchaseState != 0)
                {
                    result = new ValidationIapResult(false, "purchase was cancelled or refunded");
                }
                else
                {
                    result = new ValidationIapResult(true)
                    {
                        ValidatedReceipt = new ValidatedReceipt()
                        {
                            BundleId = receipt.BundleId,
                            ProductId = purchase.ProductId,
                            TransactionId = receipt.TransactionId,
                            OriginalTransactionId = purchase.OrderId,
                            PurchaseDateUtc = DateTime.UnixEpoch.AddMilliseconds(purchase.PurchaseTimeMillis.Value),
                            ServerUtc = DateTime.UtcNow,
                            Token = receipt.Token
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to validate product: {Message}", ex.Message);
                result = new ValidationIapResult(false, ex.Message);
            }

            return result;
        }

        private async Task<ValidationIapResult> ValidateSubscriptionAsync(Receipt receipt, ILogger log, CancellationToken cancellationToken)
        {
            var result = default(ValidationIapResult);

            try
            {
                var request = _googleService.Purchases.Subscriptionsv2.Get(receipt.BundleId, receipt.Token);
                var purchase = await request.ExecuteAsync(cancellationToken);

                receipt.Environment = purchase is null
                    ? EnvironmentType.Unknown
                    : purchase.TestPurchase is not null ? EnvironmentType.Test : EnvironmentType.Production;

                if (purchase is null)
                {
                    result = new ValidationIapResult(false, $"no purchase found");
                }
                else if (!purchase.LatestOrderId.StartsWith(receipt.TransactionId, StringComparison.Ordinal))
                {
                    result = new ValidationIapResult(false, $"transaction id '{receipt.TransactionId}' does not match '{purchase.LatestOrderId}'");
                }
                else
                {
                    var utcNow = DateTime.UtcNow;

                    var startTimeUtc = (purchase.StartTimeDateTimeOffset ?? DateTimeOffset.UnixEpoch).UtcDateTime;
                    // If the order has been cancelled, then expiry time will set to the cancel date
                    var expiryTimeUtc = purchase.LineItems
                        ?.Select(i => i.ExpiryTimeDateTimeOffset)
                        ?.Where(i => i.HasValue)
                        ?.OrderByDescending(i => i)
                        ?.FirstOrDefault()
                        ?.UtcDateTime;

                    var suspended = false;
                    var graceDays = _graceDays;

                    // Invalid states
                    switch (purchase.SubscriptionState)
                    {
                        case "SUBSCRIPTION_STATE_PENDING":
                        case "SUBSCRIPTION_STATE_PAUSED":
                        case "SUBSCRIPTION_STATE_ON_HOLD":
                            suspended = true;
                            break;
                        case "SUBSCRIPTION_STATE_CANCELED":
                            graceDays = 0;
                            break;
                    }

                    result = new ValidationIapResult(true, purchase.SubscriptionState)
                    {
                        ValidatedReceipt = new ValidatedReceipt()
                        {
                            BundleId = receipt.BundleId,
                            ProductId = receipt.ProductId,
                            TransactionId = purchase.LatestOrderId,
                            OriginalTransactionId = receipt.TransactionId,
                            PurchaseDateUtc = startTimeUtc,
                            ExpiryUtc = expiryTimeUtc,
                            ServerUtc = utcNow,
                            GraceDays = expiryTimeUtc.HasValue
                                        ? graceDays
                                        : null,
                            IsExpired = expiryTimeUtc.HasValue &&
                                        expiryTimeUtc.Value.AddDays(graceDays) <= utcNow,
                            IsSuspended = suspended,
                            Token = receipt.Token
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to validate subscription: {Message}", ex.Message);
                result = new ValidationIapResult(false, ex.Message);
            }

            return result;
        }

        private async Task<InAppProduct> GetInAppProductAsync(string bundleId, string productId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(bundleId) || string.IsNullOrEmpty(productId))
                return null;

            var result = default(InAppProduct);

            try
            {
                result = await _googleService
                    .Inappproducts
                    .Get(bundleId, productId)
                    .ExecuteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return result;
        }

        private async Task<Subscription> GetSubscriptionAsync(string bundleId, string productId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(bundleId) || string.IsNullOrEmpty(productId))
                return null;

            var result = default(Subscription);

            try
            {
                result = await _googleService
                    .Monetization
                    .Subscriptions
                    .Get(bundleId, productId)
                    .ExecuteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return result;
        }
    }
}
