using MediatR;
using Moq;
using SimpleResults;
using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class ReportRequestHandlerTests
{
    [Fact]
    public async Task GetDailyPerformanceSummaryQuery_ReturnsMockedSummaryForToday()
    {
        ReportRequestHandler handler = new(new Mock<IReportsService>().Object, new Mock<IApplicationCache>().Object);

        Result<DailyPerformanceSummaryModel> result = await handler.Handle(new ReportQueries.GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod.Today), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Period.ShouldBe(PerformanceSummaryPeriod.Today);
        result.Data.Metrics.ShouldContain(m => m.Title == "Total transaction count" && m.Value == "48");
        result.Data.DrillDownTransactions.ShouldContain(t => t.Reference == "TXN-00048");
    }
}
