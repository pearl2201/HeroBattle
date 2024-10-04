namespace Iap.Verify.Models
{
    public record ValidationIapResult
    {
        public ValidationIapResult(bool isValid, string msg)
        {
            IsValid = isValid;
            Message = msg ?? string.Empty;
        }

        public ValidationIapResult(bool isValid)
        {
            IsValid = isValid;
            Message = string.Empty;
        }

        public bool IsValid { get; init; }
        public string Message { get; init; }

        public ValidatedReceipt ValidatedReceipt { get; init; }
    }
}
