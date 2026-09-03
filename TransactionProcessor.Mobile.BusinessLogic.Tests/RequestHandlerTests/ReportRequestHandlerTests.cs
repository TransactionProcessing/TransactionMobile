using MediatR;
using Imposter.Abstractions;
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
        IReportsServiceImposter reportsService = new();
        IApplicationCacheImposter applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        applicationCache.GetMerchantDetails().Returns(merchantDetails);
        reportsService.GetDailyPerformanceSummary(
                                  PerformanceSummaryPeriod.Today,
                                  merchantDetails.MerchantReportingId,
                                  Arg<DateTime>.Any(),
                                  Arg<DateTime>.Any(),
                                  Arg<CancellationToken>.Any())
                      .ReturnsAsync(Result.Success(DailyPerformanceSummaryModel.CreateMock(PerformanceSummaryPeriod.Today)));

        ReportRequestHandler handler = new(reportsService.Instance(), applicationCache.Instance());

        Result<DailyPerformanceSummaryModel> result = await handler.Handle(new ReportQueries.GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod.Today), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Period.ShouldBe(PerformanceSummaryPeriod.Today);
        result.Data.Metrics.ShouldContain(m => m.Title == "Total transaction count" && m.Value == "48");
        result.Data.DrillDownTransactions.ShouldContain(t => t.Reference == "TXN-00048");
        reportsService.GetDailyPerformanceSummary(
                                  PerformanceSummaryPeriod.Today,
                                  merchantDetails.MerchantReportingId,
                                  Arg<DateTime>.Any(),
                                  Arg<DateTime>.Any(),
                                  Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetDailyPerformanceSummaryQuery_InvalidPeriod_ReturnsFailure()
    {
        IReportsServiceImposter reportsService = new();
        IApplicationCacheImposter applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        applicationCache.GetMerchantDetails().Returns(merchantDetails);

        ReportRequestHandler handler = new(reportsService.Instance(), applicationCache.Instance());

        Result<DailyPerformanceSummaryModel> result = await handler.Handle(
            new ReportQueries.GetDailyPerformanceSummaryQuery((PerformanceSummaryPeriod)999),
            CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        reportsService.GetDailyPerformanceSummary(
                                  Arg<PerformanceSummaryPeriod>.Any(),
                                  Arg<Int32>.Any(),
                                  Arg<DateTime>.Any(),
                                  Arg<DateTime>.Any(),
                                  Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task GetTransactionMixSummaryQuery_ReturnsRequestedBreakdown()
    {
        IReportsServiceImposter reportsService = new();
        IApplicationCacheImposter applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        applicationCache.GetMerchantDetails().Returns(merchantDetails);
        reportsService.GetTransactionMixSummary(
                                  merchantDetails.MerchantReportingId,
                                  Arg<DateTime>.Any(),
                                  Arg<DateTime>.Any(),
                                  TransactionMixBreakdown.Product,
                                  TransactionMixMeasure.Value,
                                  5,
                                  Arg<CancellationToken>.Any())
                      .ReturnsAsync(Result.Success(TransactionMixSummaryModel.CreateMock(
                          merchantReportingId: merchantDetails.MerchantReportingId,
                          breakdown: TransactionMixBreakdown.Product,
                          measure: TransactionMixMeasure.Value)));

        ReportRequestHandler handler = new(reportsService.Instance(), applicationCache.Instance());

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
        reportsService.GetTransactionMixSummary(
                                  merchantDetails.MerchantReportingId,
                                  new DateTime(2026, 7, 1),
                                  new DateTime(2026, 7, 31),
                                  TransactionMixBreakdown.Product,
                                  TransactionMixMeasure.Value,
                                  5,
                                  Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetRecentActivityReceiptReportQuery_ReturnsMockedResultsForOneDate()
    {
        IReportsServiceImposter reportsService = new();
        IApplicationCacheImposter applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        DateTime reportDate = new(2026, 7, 6);

        applicationCache.GetMerchantDetails().Returns(merchantDetails);
        reportsService.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  "TXN-10001",
                                  1,
                                  5,
                                  Arg<CancellationToken>.Any())
                      .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, "TXN-10001")));

        ReportRequestHandler handler = new(reportsService.Instance(), applicationCache.Instance());

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
        reportsService.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  "TXN-10001",
                                  1,
                                  5,
                                  Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task GetRecentActivityReceiptReportQuery_PassesPagingToService()
    {
        IReportsServiceImposter reportsService = new();
        IApplicationCacheImposter applicationCache = new();
        MerchantDetailsModel merchantDetails = new()
        {
            MerchantReportingId = 12345
        };

        DateTime reportDate = new(2026, 7, 6);

        applicationCache.GetMerchantDetails().Returns(merchantDetails);
        reportsService.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  Arg<String?>.Any(),
                                  2,
                                  5,
                                  Arg<CancellationToken>.Any())
                      .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null)));

        ReportRequestHandler handler = new(reportsService.Instance(), applicationCache.Instance());

        Result<RecentActivityReceiptReportModel> result = await handler.Handle(
            new ReportQueries.GetRecentActivityReceiptReportQuery(
                ReportDate: reportDate,
                SearchText: null,
                PageNumber: 2,
                PageSize: 5),
            CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        reportsService.GetRecentActivityReceiptReport(
                                  merchantDetails.MerchantReportingId,
                                  reportDate,
                                  Arg<String?>.Any(),
                                  2,
                                  5,
                                  Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
