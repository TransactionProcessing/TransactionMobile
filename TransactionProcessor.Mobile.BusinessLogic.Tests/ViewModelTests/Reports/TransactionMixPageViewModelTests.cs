using MediatR;
using Imposter.Abstractions;
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
    private readonly IMediatorImposter Mediator;
    private readonly INavigationServiceImposter NavigationService;
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly IDialogServiceImposter DialogService;
    private readonly IDeviceServiceImposter DeviceService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;
    private readonly TransactionMixPageViewModel ViewModel;

    public TransactionMixPageViewModelTests()
    {
        this.Mediator = new IMediatorImposter();
        this.Mediator
            .Send(Arg<IRequest<Result<TransactionMixSummaryModel>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(TransactionMixSummaryModel.CreateMock(
                merchantReportingId: 12345,
                breakdown: TransactionMixBreakdown.TransactionType,
                measure: TransactionMixMeasure.Count)));

        this.NavigationService = new INavigationServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();

        this.ViewModel = new TransactionMixPageViewModel(this.Mediator.Instance(),
                                                         this.NavigationService.Instance(),
                                                         this.ApplicationCache.Instance(),
                                                         this.DialogService.Instance(),
                                                         this.DeviceService.Instance(),
                                                         this.NavigationParameterService.Instance());
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
        this.ViewModel.ChartSeries.Length.ShouldBeGreaterThan(0);
        this.ViewModel.ChartYAxes.Count.ShouldBe(0);
        this.ViewModel.ChartXAxes.Count.ShouldBe(0);
        this.ViewModel.ChartSubtitle.ShouldContain("Transaction Type");
        this.ViewModel.ChartSubtitle.ShouldContain("count");
        this.ViewModel.IsLoading.ShouldBeFalse();
    }
}
