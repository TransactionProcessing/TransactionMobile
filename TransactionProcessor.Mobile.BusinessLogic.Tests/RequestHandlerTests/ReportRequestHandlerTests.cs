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

    [Fact]
    public async Task GetTransactionMixSummaryQuery_ReturnsRequestedBreakdown()
    {
        Mock<IReportsService> reportsService = new();
        Mock<IApplicationCache> applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(merchantDetails);
        reportsService.Setup(r => r.GetTransactionMixSummary(
                                  merchantDetails.MerchantReportingId,
                                  It.IsAny<DateTime>(),
                                  It.IsAny<DateTime>(),
                                  TransactionMixBreakdown.Product,
                                  TransactionMixMeasure.Value,
                                  5,
                                  It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Result.Success(TransactionMixSummaryModel.CreateMock(
                          merchantReportingId: merchantDetails.MerchantReportingId,
                          breakdown: TransactionMixBreakdown.Product,
                          measure: TransactionMixMeasure.Value)));

        ReportRequestHandler handler = new(reportsService.Object, applicationCache.Object);

        Result<TransactionMixSummaryModel> result = await handler.Handle(
            new ReportQueries.GetTransactionMixSummaryQuery(
                StartDate: new DateTime(2026, 7, 1),
                EndDate: new DateTime(2026, 7, 31),
                Breakdown: TransactionMixBreakdown.Product,
                Measure: TransactionMixMeasure.Value,
                TopN: 5),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Breakdown.ShouldBe(TransactionMixBreakdown.Product);
        result.Data.Measure.ShouldBe(TransactionMixMeasure.Value);
        result.Data.Items.ShouldNotBeEmpty();
        reportsService.Verify(r => r.GetTransactionMixSummary(
                                  merchantDetails.MerchantReportingId,
                                  new DateTime(2026, 7, 1),
                                  new DateTime(2026, 7, 31),
                                  TransactionMixBreakdown.Product,
                                  TransactionMixMeasure.Value,
                                  5,
                                  It.IsAny<CancellationToken>()),
                              Times.Once);
    }

    [Fact]
    public async Task GetRecentActivityReceiptReportQuery_ReturnsMockedResultsForOneDate()
    {
        Mock<IReportsService> reportsService = new();
        Mock<IApplicationCache> applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        DateTime reportDate = new(2026, 7, 6);

        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(merchantDetails);
        reportsService.Setup(r => r.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  "TXN-10001",
                                  1,
                                  5,
                                  It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, "TXN-10001")));

        ReportRequestHandler handler = new(reportsService.Object, applicationCache.Object);

        Result<RecentActivityReceiptReportModel> result = await handler.Handle(
            new ReportQueries.GetRecentActivityReceiptReportQuery(
                ReportDate: reportDate,
                SearchText: "TXN-10001",
                PageNumber: 1,
                PageSize: 5),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ReportDate.ShouldBe(reportDate);
        result.Data.Items.ShouldNotBeEmpty();
        result.Data.Items.All(item => item.TransactionDateTime.Date == reportDate).ShouldBeTrue();
        reportsService.Verify(r => r.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  "TXN-10001",
                                  1,
                                  5,
                                  It.IsAny<CancellationToken>()),
                              Times.Once);
    }

    [Fact]
    public async Task GetRecentActivityReceiptReportQuery_PassesPagingToService()
    {
        Mock<IReportsService> reportsService = new();
        Mock<IApplicationCache> applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        DateTime reportDate = new(2026, 7, 6);

        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(merchantDetails);
        reportsService.Setup(r => r.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  null,
                                  2,
                                  5,
                                  It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null)));

        ReportRequestHandler handler = new(reportsService.Object, applicationCache.Object);

        Result<RecentActivityReceiptReportModel> result = await handler.Handle(
            new ReportQueries.GetRecentActivityReceiptReportQuery(
                ReportDate: reportDate,
                SearchText: null,
                PageNumber: 2,
                PageSize: 5),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        reportsService.Verify(r => r.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  null,
                                  2,
                                  5,
                                  It.IsAny<CancellationToken>()),
                              Times.Once);
    }
}
