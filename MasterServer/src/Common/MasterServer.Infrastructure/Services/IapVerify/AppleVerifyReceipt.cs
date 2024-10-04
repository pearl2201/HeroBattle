using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Iap.Verify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Iap.Verify
{
    public class AppleVerifyReceipt : Apple
    {
        private const string AppleProductionUrl = "https://buy.itunes.apple.com/verifyReceipt";
        private const string AppleTestUrl = "https://sandbox.itunes.apple.com/verifyReceipt";
        private const string ValidatorRoute = "v1/Apple";

        private readonly ILogger _logger;

        private readonly AppleSecretOptions _secretOptions;

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly int _graceDays;

        public AppleVerifyReceipt(
            IOptions<AppleSecretOptions> secretOptions,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IApplicationDbContext dbContext,
            ILoggerFactory loggerFactory) : base(dbContext)
        {
            _logger = loggerFactory.CreateLogger<AppleVerifyReceipt>();

            _secretOptions = secretOptions.Value;

            _httpClient = httpClientFactory.CreateClient();

            _jsonSerializerOptions = new JsonSerializerOptions(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            });

            _ = int.TryParse(configuration[GraceDays], out _graceDays);
        }

        public override async Task<ServiceResult<ValidationIapResult>> Run(long playerId, Receipt receipt, CancellationToken cancellationToken)
        {
            var result = default(ValidationIapResult);

            if (receipt?.IsValid() == true)
            {
                var appleResponse = await PostAppleReceiptAsync(AppleProductionUrl, receipt, _logger, cancellationToken);
                // Apple recommends calling production, then falling back to sandbox on an error code
                if (appleResponse?.WrongEnvironment == true)
                {
                    _logger.LogInformation("Sandbox purchase, calling test environment...");
                    appleResponse = await PostAppleReceiptAsync(AppleTestUrl, receipt, _logger, cancellationToken);
                }

                if (appleResponse?.IsValid == true)
                {
                    result = ValidateProduct(receipt, appleResponse, _logger);
                }
                else if (!string.IsNullOrEmpty(appleResponse?.Error))
                {
                    result = new ValidationIapResult(false, appleResponse.Error);
                }
                else
                {
                    result = new ValidationIapResult(false, $"Invalid {nameof(Receipt)}");
                }
            }
            else
            {
                result = new ValidationIapResult(false, $"Invalid {nameof(Receipt)}");
            }

            return LogVerificationResultAsync(playerId, ValidatorRoute, receipt, result, _logger, cancellationToken);
        }

        private async Task<AppleResponse> PostAppleReceiptAsync(string url, Receipt receipt, ILogger log, CancellationToken cancellationToken)
        {
            var appleResponse = default(AppleResponse);

            _secretOptions.Secrets.TryGetValue(receipt.BundleId, out var appSecret);
            if (string.IsNullOrEmpty(appSecret))
                appSecret = _secretOptions.Master;

            if (!string.IsNullOrEmpty(appSecret))
            {
                try
                {
                    var request = new AppleRequest()
                    {
                        ReceiptData = receipt.Token,
                        Password = appSecret
                    };

                    var postBody = new StringContent(JsonSerializer.Serialize(request));
                    using var response = await _httpClient.PostAsync(url, postBody, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    // Expects an iOS 7 style receipt
                    appleResponse = await JsonSerializer.DeserializeAsync<AppleResponse>(stream, options: _jsonSerializerOptions, cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "Failed to parse AppleResponse: {Message}", ex.Message);
                }
            }

            return appleResponse;
        }

        private ValidationIapResult ValidateProduct(Receipt receipt, AppleResponse appleResponse, ILogger log)
        {
            var result = default(ValidationIapResult);

            try
            {
                receipt.Environment = string.Equals(appleResponse.Environment, Production, StringComparison.OrdinalIgnoreCase)
                    ? EnvironmentType.Production
                    : EnvironmentType.Test;

                if (appleResponse.Receipt is null)
                {
                    result = new ValidationIapResult(false, "no receipt returned");
                }
                else if (!string.Equals(appleResponse.Receipt.BundleId, receipt.BundleId, StringComparison.Ordinal))
                {
                    result = new ValidationIapResult(false, $"bundle id '{receipt.BundleId}' does not match '{appleResponse.Receipt.BundleId}'");
                }
                else
                {
                    var purchases = appleResponse.LatestReceiptInfo?.Any() == true
                        ? appleResponse.LatestReceiptInfo.OfType<IAppleInApp>()
                        : appleResponse.Receipt?.InApp?.OfType<IAppleInApp>();
                    var purchase = purchases
                        ?.Where(p => p.ProductId == receipt.ProductId)
                        ?.OrderByDescending(p => long.TryParse(p.PurchaseDateMs, out var ms) ? ms : long.MaxValue)
                        ?.FirstOrDefault();

                    if (purchase is null)
                    {
                        result = new ValidationIapResult(false, $"did not find '{receipt.ProductId}' in list of purchases");
                    }
                    else
                    {
                        var utcNow = DateTime.UtcNow;

                        var purchaseDateUtc = purchase.GetPurchaseDateUtc();
                        var expiresDateUtc = purchase.GetExpiresDateUtc();
                        var cancellationDateUtc = purchase.GetCancellationDateUtc();
                        var graceDays = _graceDays;

                        var msg = string.Empty;

                        if (cancellationDateUtc.HasValue)
                        {
                            msg = "App Store refunded a transaction or revoked it from family sharing";
                            expiresDateUtc = cancellationDateUtc;
                            graceDays = 0;
                        }

                        result = new ValidationIapResult(true, msg)
                        {
                            ValidatedReceipt = new ValidatedReceipt()
                            {
                                BundleId = receipt.BundleId,
                                ProductId = receipt.ProductId,
                                TransactionId = purchase.TransactionId,
                                OriginalTransactionId = purchase.OriginalTransactionId,
                                PurchaseDateUtc = purchaseDateUtc,
                                ExpiryUtc = expiresDateUtc,
                                ServerUtc = utcNow,
                                GraceDays = expiresDateUtc.HasValue
                                            ? graceDays
                                            : null,
                                IsExpired = expiresDateUtc.HasValue &&
                                            expiresDateUtc.Value.AddDays(graceDays) <= utcNow,
                                IsSuspended = false,
                                Token = receipt.Token
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to validate product: {Message}", ex.Message);
                result = new ValidationIapResult(false, ex.Message);
            }

            return result;
        }
    }
}
