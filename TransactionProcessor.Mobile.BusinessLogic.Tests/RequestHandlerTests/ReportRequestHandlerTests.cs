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
        Mock<IReportsService> reportsService = new();
        Mock<IApplicationCache> applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(merchantDetails);
        reportsService.Setup(r => r.GetDailyPerformanceSummary(
                                  PerformanceSummaryPeriod.Today,
                                  merchantDetails.MerchantReportingId,
                                  It.IsAny<DateTime>(),
                                  It.IsAny<DateTime>(),
                                  It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Result.Success(DailyPerformanceSummaryModel.CreateMock(PerformanceSummaryPeriod.Today)));

        ReportRequestHandler handler = new(reportsService.Object, applicationCache.Object);

        Result<DailyPerformanceSummaryModel> result = await handler.Handle(new ReportQueries.GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod.Today), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Period.ShouldBe(PerformanceSummaryPeriod.Today);
        result.Data.Metrics.ShouldContain(m => m.Title == "Total transaction count" && m.Value == "48");
        result.Data.DrillDownTransactions.ShouldContain(t => t.Reference == "TXN-00048");
        reportsService.Verify(r => r.GetDailyPerformanceSummary(
                                  PerformanceSummaryPeriod.Today,
                                  merchantDetails.MerchantReportingId,
                                  It.IsAny<DateTime>(),
                                  It.IsAny<DateTime>(),
                                  It.IsAny<CancellationToken>()),
                              Times.Once);
    }
}
