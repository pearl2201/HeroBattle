﻿namespace Iap.Verify.Models
{
    public class ValidatedReceipt
    {
        public string BundleId { get; set; }
        public string ProductId { get; set; }

        public string TransactionId { get; set; }
        public string OriginalTransactionId { get; set; }

        public DateTime PurchaseDateUtc { get; set; }
        public DateTime? ExpiryUtc { get; set; }

        public DateTime ServerUtc { get; set; }
        public int? GraceDays { get; set; }
        public bool IsExpired { get; set; }
        public bool IsSuspended { get; set; }

        public string Token { get; set; }
    }
}
