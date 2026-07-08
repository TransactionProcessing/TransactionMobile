namespace TransactionProcessor.Mobile.BusinessLogic.Models;

public sealed record RecentActivityReceiptReportModel
{
    public DateTime ReportDate { get; set; }

    public string? SearchText { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public IReadOnlyList<RecentActivityReceiptItemModel> Items { get; set; } = [];

    public int TotalPages => this.PageSize <= 0
        ? 0
        : (int)Math.Ceiling(this.TotalCount / (decimal)this.PageSize);

    public static RecentActivityReceiptReportModel CreateMock(DateTime reportDate,
                                                              string? searchText,
                                                              int pageNumber = 1,
                                                              int pageSize = 5)
    {
        List<RecentActivityReceiptItemModel> items =
        [
            new("TXN-10001", "Mobile Topup", "Custom", "Safaricom", "Success", 100.00m, reportDate.Date.AddHours(9).AddMinutes(30), "RCPT-10001"),
            new("TXN-10002", "Bill Payment", "Bill Pay (Post)", "PataPawa PostPay", "Success", 250.00m, reportDate.Date.AddHours(10).AddMinutes(15), "RCPT-10002"),
            new("TXN-10003", "Voucher Issue", "10 KES", "Voucher", "Failed", 0.00m, reportDate.Date.AddHours(11).AddMinutes(5), "RCPT-10003"),
            new("TXN-10004", "Mobile Topup", "Airtime", "Airtel", "Success", 50.00m, reportDate.Date.AddHours(12).AddMinutes(20), "RCPT-10004"),
            new("TXN-10005", "Bill Payment", "Prepaid Power", "KPLC", "Failed", 75.00m, reportDate.Date.AddHours(13).AddMinutes(5), "RCPT-10005"),
            new("TXN-10006", "Voucher Issue", "20 KES", "Voucher", "Success", 20.00m, reportDate.Date.AddHours(14).AddMinutes(10), "RCPT-10006"),
            new("TXN-10007", "Mobile Topup", "Custom", "Safaricom", "Success", 500.00m, reportDate.Date.AddHours(15).AddMinutes(55), "RCPT-10007"),
        ];

        if (string.IsNullOrWhiteSpace(searchText) == false)
        {
            string term = searchText.Trim();
            items = items.Where(item => item.Matches(term)).ToList();
        }

        items = items.OrderByDescending(item => item.TransactionDateTime).ToList();

        int normalizedPageSize = pageSize > 0 ? pageSize : 5;
        int normalizedPageNumber = pageNumber > 0 ? pageNumber : 1;
        int totalCount = items.Count;

        return new RecentActivityReceiptReportModel
        {
            ReportDate = reportDate.Date,
            SearchText = searchText,
            PageNumber = normalizedPageNumber,
            PageSize = normalizedPageSize,
            TotalCount = totalCount,
            Items = items.Skip((normalizedPageNumber - 1) * normalizedPageSize)
                         .Take(normalizedPageSize)
                         .ToList(),
        };
    }
}

public sealed record RecentActivityReceiptItemModel(string Reference,
                                                    string TransactionType,
                                                    string Product,
                                                    string Operator,
                                                    string Status,
                                                    decimal Amount,
                                                    DateTime TransactionDateTime,
                                                    string ReceiptReference)
{
    public bool Matches(string searchText)
    {
        return this.Reference.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            || this.TransactionType.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            || this.Product.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            || this.Operator.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            || this.Status.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            || this.ReceiptReference.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            || this.Amount.ToString("0.##").Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }
}
