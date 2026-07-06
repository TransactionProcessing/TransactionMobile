namespace TransactionProcessor.Mobile.BusinessLogic.Models;

public sealed record TransactionMixSummaryModel
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public TransactionMixBreakdown Breakdown { get; set; }

    public TransactionMixMeasure Measure { get; set; }

    public decimal TotalCount { get; set; }

    public decimal TotalValue { get; set; }

    public List<TransactionMixSummaryItemModel> Items { get; set; } = [];

    public List<TransactionMixDrillDownTransactionModel> DrillDownTransactions { get; set; } = [];

    public static TransactionMixSummaryModel CreateMock(int merchantReportingId,
                                                        TransactionMixBreakdown breakdown,
                                                        TransactionMixMeasure measure)
    {
        DateTime today = DateTime.Today;

        List<TransactionMixSummaryItemModel> items = breakdown switch
        {
            TransactionMixBreakdown.Product => new List<TransactionMixSummaryItemModel>
            {
                new("custom", "Custom", 12, 1250.00m),
                new("bill-pay-post", "Bill Pay (Post)", 8, 1900.00m),
                new("10-kes", "10 KES", 6, 60.00m),
            },
            TransactionMixBreakdown.Operator => new List<TransactionMixSummaryItemModel>
            {
                new("safaricom", "Safaricom", 18, 1800.00m),
                new("voucher", "Voucher", 6, 60.00m),
                new("patapawa-postpay", "PataPawa PostPay", 4, 1500.00m),
            },
            TransactionMixBreakdown.Status => new List<TransactionMixSummaryItemModel>
            {
                new("success", "Success", 22, 3110.00m),
                new("failed", "Failed", 6, 0.00m),
            },
            _ => new List<TransactionMixSummaryItemModel>
            {
                new("mobile-topup", "Mobile Topup", 14, 1250.00m),
                new("bill-payment", "Bill Payment", 10, 1900.00m),
                new("voucher-issue", "Voucher Issue", 4, 60.00m),
            }
        };

        List<TransactionMixDrillDownTransactionModel> transactions = new()
        {
            new("TXN-10001", "Mobile Topup", "Custom", "Safaricom", "Success", 100.00m, today.AddHours(-1)),
            new("TXN-10002", "Bill Payment", "Bill Pay (Post)", "PataPawa PostPay", "Success", 250.00m, today.AddHours(-2)),
            new("TXN-10003", "Voucher Issue", "10 KES", "Voucher", "Failed", 0.00m, today.AddHours(-3)),
        };

        return new TransactionMixSummaryModel
        {
            Breakdown = breakdown,
            DrillDownTransactions = transactions,
            FromDate = today.AddDays(-7),
            ToDate = today,
            Items = items,
            Measure = measure,
            TotalCount = 28,
            TotalValue = 3110.00m,
        };
    }
}

public sealed record TransactionMixSummaryItemModel(string Key,
                                                    string Label,
                                                    decimal Count,
                                                    decimal Value);

public sealed record TransactionMixDrillDownTransactionModel(string Reference,
                                                             string TransactionType,
                                                             string Product,
                                                             string Operator,
                                                             string Status,
                                                             decimal Amount,
                                                             DateTime TransactionDateTime);
