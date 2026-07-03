using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;

public sealed class ReportRequestHandler : IRequestHandler<ReportQueries.GetDailyPerformanceSummaryQuery, Result<DailyPerformanceSummaryModel>> {
    private readonly IReportsService ReportsService;
    private readonly IApplicationCache ApplicationCache;

    public ReportRequestHandler(IReportsService reportsService, IApplicationCache applicationCache) {
        ReportsService = reportsService;
        this.ApplicationCache = applicationCache;
    }


    public async Task<Result<DailyPerformanceSummaryModel>> Handle(ReportQueries.GetDailyPerformanceSummaryQuery request,
                                                                   CancellationToken cancellationToken) {

        DateTime current = DateTime.Now.Date;

        (DateTime startDate, DateTime endDate) dates = request.Period switch {
            PerformanceSummaryPeriod.Today => (current, current),
            PerformanceSummaryPeriod.Yesterday => (current.AddDays(-1), current.AddDays(-1)),
            PerformanceSummaryPeriod.ThisWeek => (DailyPerformanceSummaryModel.StartOfWeek(current), current),
            PerformanceSummaryPeriod.MonthToDate => (new DateTime(current.Year, current.Month, 1), current),
            _ => throw new ArgumentOutOfRangeException(nameof(request.Period), request.Period, "Invalid performance summary period.")
        };

        MerchantDetailsModel merchant = this.ApplicationCache.GetMerchantDetails();
        
        return await this.ReportsService.GetDailyPerformanceSummary(request.Period, merchant.MerchantReportingId, dates.startDate, dates.endDate, cancellationToken);
    }
}
