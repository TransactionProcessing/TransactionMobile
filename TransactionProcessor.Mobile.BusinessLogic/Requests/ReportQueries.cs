using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record ReportQueries
{
    public record GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod Period) : IRequest<Result<DailyPerformanceSummaryModel>>;

    public record GetTransactionMixSummaryQuery(DateTime StartDate,
                                                DateTime EndDate,
                                                TransactionMixBreakdown Breakdown,
                                                TransactionMixMeasure Measure,
                                                int TopN) : IRequest<Result<TransactionMixSummaryModel>>;

    public record GetRecentActivityReceiptReportQuery(DateTime ReportDate,
                                                      string? SearchText,
                                                      int PageNumber = 1,
                                                      int PageSize = 5) : IRequest<Result<RecentActivityReceiptReportModel>>;
}
