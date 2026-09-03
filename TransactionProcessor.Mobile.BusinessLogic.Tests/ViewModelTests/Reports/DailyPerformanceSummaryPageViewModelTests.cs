using MediatR;
using Imposter.Abstractions;
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
    private readonly IMediatorImposter Mediator;
    private readonly INavigationServiceImposter NavigationService;
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly IDialogServiceImposter DialogService;
    private readonly IDeviceServiceImposter DeviceService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;
    private readonly DailyPerformanceSummaryPageViewModel ViewModel;

    public DailyPerformanceSummaryPageViewModelTests()
    {
        this.Mediator = new IMediatorImposter();
        this.Mediator
            .Send(Arg<IRequest<Result<DailyPerformanceSummaryModel>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(DailyPerformanceSummaryModel.CreateMock(PerformanceSummaryPeriod.Today)));
        this.NavigationService = new INavigationServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();

        this.ViewModel = new DailyPerformanceSummaryPageViewModel(this.Mediator.Instance(),
                                                                  this.NavigationService.Instance(),
                                                                  this.ApplicationCache.Instance(),
                                                                  this.DialogService.Instance(),
                                                                  this.DeviceService.Instance(),
                                                                  this.NavigationParameterService.Instance());
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
