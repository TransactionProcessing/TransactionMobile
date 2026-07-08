namespace TransactionProcessor.Mobile.BusinessLogic.Models;

public sealed record RecentActivityReceiptResendResultModel
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public string? Reference { get; set; }

    public string? ReceiptReference { get; set; }

    public string? TransactionReference { get; set; }
}
