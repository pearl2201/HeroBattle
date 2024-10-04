using Iap.Verify.Models;
using MasterServer.Domain.Utils;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Economy
{
    public class PlayerShopRealMoneyPurchaseReceipt : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public long PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public string PurchaseDefinitionId { get; set; }
        [ForeignKey("PurchaseDefinitionId")]
        public RealMoneyPurchaseDefinition PurchaseDefinition { get; set; }
        [Column(TypeName = "jsonb")]
        public List<RewardItemValueObject> Rewards { get; set; }

        public Instant DateVerified { get; set; }

        public string BundleId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }

        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string Environment { get; set; }
        public string AppVersion { get; set; }

        public string ValidatorName { get; set; }

        public void SetValidateResult(Receipt receipt, ValidationIapResult validationResult, string validatorName = "")
        {
            DateVerified = DateTimeHelper.InstanceNow;
            BundleId = receipt.BundleId ?? string.Empty;
            TransactionId = receipt.TransactionId ?? string.Empty;
            Token = receipt.Token ?? string.Empty;
            IsValid = validationResult.IsValid;
            Message = validationResult.Message ?? string.Empty;
            Environment = receipt.Environment != EnvironmentType.Unknown ? receipt.Environment.ToString() : string.Empty;
            AppVersion = receipt.AppVersion ?? string.Empty;
            ValidatorName = validatorName;
        }

    }
}
