using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;

public sealed class ReportRequestHandler : IRequestHandler<ReportQueries.GetDailyPerformanceSummaryQuery, Result<DailyPerformanceSummaryModel>>
{
    public Task<Result<DailyPerformanceSummaryModel>> Handle(ReportQueries.GetDailyPerformanceSummaryQuery request, CancellationToken cancellationToken)
    {
        DailyPerformanceSummaryModel summary = DailyPerformanceSummaryModel.CreateMock(request.Period);
        return Task.FromResult(Result.Success(summary));
    }
}
