namespace TransactionProcessor.Mobile.BusinessLogic.Models;

public enum PerformanceSummaryPeriod
{
    Today = 0,
    Yesterday = 1,
    ThisWeek = 2,
    MonthToDate = 3,
}

public sealed record DailyPerformanceSummaryModel(
    PerformanceSummaryPeriod Period,
    DateTime FromDate,
    DateTime ToDate,
    string PeriodLabel,
    IReadOnlyList<DailyPerformanceMetricModel> Metrics,
    IReadOnlyList<DailyPerformanceTransactionModel> DrillDownTransactions)
{
    public static DailyPerformanceSummaryModel CreateMock(PerformanceSummaryPeriod period)
    {
        DateTime today = DateTime.Today;
        DateTime fromDate = period switch
        {
            PerformanceSummaryPeriod.Today => today,
            PerformanceSummaryPeriod.Yesterday => today.AddDays(-1),
            PerformanceSummaryPeriod.ThisWeek => StartOfWeek(today),
            PerformanceSummaryPeriod.MonthToDate => new DateTime(today.Year, today.Month, 1),
            _ => today,
        };

        DateTime toDate = period switch
        {
            PerformanceSummaryPeriod.Yesterday => today.AddDays(-1),
            _ => today,
        };

        List<DailyPerformanceMetricModel> metrics = new()
        {
            new DailyPerformanceMetricModel("Total transaction count", "48", "Processed today", DailyPerformanceMetricCategory.Total),
            new DailyPerformanceMetricModel("Total transaction value", "10,250.00 KES", "Gross value", DailyPerformanceMetricCategory.Total),
            new DailyPerformanceMetricModel("Successful transaction count", "44", "Completed successfully", DailyPerformanceMetricCategory.Success),
            new DailyPerformanceMetricModel("Failed transaction count", "4", "Could not be completed", DailyPerformanceMetricCategory.Failure),
            new DailyPerformanceMetricModel("Average transaction value", "213.54 KES", "Average across all transactions"),
            new DailyPerformanceMetricModel("Top product", "Mobile Topup", "Highest volume"),
        };

        List<DailyPerformanceTransactionModel> drillDownTransactions = new()
        {
            new DailyPerformanceTransactionModel("TXN-00048", "Mobile Topup", "Success", "250.00 KES", today.AddHours(-1)),
            new DailyPerformanceTransactionModel("TXN-00047", "Bill Payment", "Success", "1,500.00 KES", today.AddHours(-2)),
            new DailyPerformanceTransactionModel("TXN-00046", "Voucher Issue", "Failed", "0.00 KES", today.AddHours(-3)),
        };

        return new DailyPerformanceSummaryModel(period, fromDate, toDate, period.ToString(), metrics, drillDownTransactions);
    }

    private static DateTime StartOfWeek(DateTime date)
    {
        int daysSinceSunday = (int)date.DayOfWeek;
        return date.Date.AddDays(-daysSinceSunday);
    }
}

public enum DailyPerformanceMetricCategory
{
    Neutral = 0,
    Total = 1,
    Success = 2,
    Failure = 3,
}

public sealed record DailyPerformanceMetricModel(string Title, string Value, string Description, DailyPerformanceMetricCategory Category = DailyPerformanceMetricCategory.Neutral);

public sealed record DailyPerformanceTransactionModel(string Reference, string Product, string Status, string Amount, DateTime TransactionDateTime);
