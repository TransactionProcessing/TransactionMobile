using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

public record ReportQueries
{
    public record GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod Period) : IRequest<Result<DailyPerformanceSummaryModel>>;
}
