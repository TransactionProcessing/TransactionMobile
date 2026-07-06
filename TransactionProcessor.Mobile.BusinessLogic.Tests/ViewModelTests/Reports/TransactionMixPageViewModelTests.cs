using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Reports;

public class TransactionMixPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogService;
    private readonly Mock<IDeviceService> DeviceService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;
    private readonly TransactionMixPageViewModel ViewModel;

    public TransactionMixPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        this.Mediator
            .Setup(m => m.Send(It.IsAny<ReportQueries.GetTransactionMixSummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(TransactionMixSummaryModel.CreateMock(
                merchantReportingId: 12345,
                breakdown: TransactionMixBreakdown.TransactionType,
                measure: TransactionMixMeasure.Count)));

        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();

        this.ViewModel = new TransactionMixPageViewModel(this.Mediator.Object,
                                                         this.NavigationService.Object,
                                                         this.ApplicationCache.Object,
                                                         this.DialogService.Object,
                                                         this.DeviceService.Object,
                                                         this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task Initialise_LoadsDefaultTransactionMixSummary()
    {
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.Title.ShouldBe("Transaction Mix");
        this.ViewModel.SelectedBreakdown.ShouldBe(TransactionMixBreakdown.TransactionType);
        this.ViewModel.SelectedMeasure.ShouldBe(TransactionMixMeasure.Count);
        this.ViewModel.Summary.ShouldNotBeNull();
        this.ViewModel.Items.Count.ShouldBeGreaterThan(0);
        this.ViewModel.TopItems.Count.ShouldBeGreaterThan(0);
        this.ViewModel.HasChartData.ShouldBeTrue();
        this.ViewModel.ChartSeries.Length.ShouldBe(1);
        this.ViewModel.ChartYAxes.Count.ShouldBe(1);
        this.ViewModel.ChartXAxes.Count.ShouldBe(1);
        this.ViewModel.IsLoading.ShouldBeFalse();
    }
}
