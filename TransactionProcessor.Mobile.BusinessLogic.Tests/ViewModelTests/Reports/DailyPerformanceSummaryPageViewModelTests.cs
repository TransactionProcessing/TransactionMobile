using MediatR;
using Moq;
using SimpleResults;
using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Reports;

public class DailyPerformanceSummaryPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogService;
    private readonly Mock<IDeviceService> DeviceService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;
    private readonly DailyPerformanceSummaryPageViewModel ViewModel;

    public DailyPerformanceSummaryPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        this.Mediator
            .Setup(m => m.Send(It.IsAny<ReportQueries.GetDailyPerformanceSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(DailyPerformanceSummaryModel.CreateMock(PerformanceSummaryPeriod.Today)));
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();

        this.ViewModel = new DailyPerformanceSummaryPageViewModel(this.Mediator.Object,
                                                                  this.NavigationService.Object,
                                                                  this.ApplicationCache.Object,
                                                                  this.DialogService.Object,
                                                                  this.DeviceService.Object,
                                                                  this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task Initialise_LoadsMockedSummaryForToday()
    {
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.SelectedPeriod.ShouldBe(PerformanceSummaryPeriod.Today);
        this.ViewModel.SummaryCards.Count.ShouldBe(6);
        this.ViewModel.TopSummaryCards.Count.ShouldBe(4);
        this.ViewModel.TopSummaryCardsRow1.Count.ShouldBe(2);
        this.ViewModel.TopSummaryCardsRow2.Count.ShouldBe(2);
        this.ViewModel.TopSummaryCardsRow1.Select(card => card.Title).ShouldBe(new[]
        {
            "Total transaction count",
            "Total transaction value",
        });
        this.ViewModel.TopSummaryCardsRow2.Select(card => card.Title).ShouldBe(new[]
        {
            "Successful transaction count",
            "Failed transaction count",
        });
        this.ViewModel.DrillDownTransactions.Count.ShouldBe(3);
        this.ViewModel.IsLoading.ShouldBeFalse();
    }
}
