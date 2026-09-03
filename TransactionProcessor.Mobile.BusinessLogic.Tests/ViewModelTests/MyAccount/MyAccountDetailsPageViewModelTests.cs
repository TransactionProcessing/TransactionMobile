using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.MyAccount;

[Collection("ViewModelTests")]
public class MyAccountDetailsPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;
    private INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogService;

    private readonly MyAccountDetailsPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    private readonly IMediatorImposter Mediator;

    public MyAccountDetailsPageViewModelTests()
    {
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.Mediator = new IMediatorImposter();
        this.ViewModel = new MyAccountDetailsPageViewModel(this.NavigationService.Instance(),
                                                           this.ApplicationCache.Instance(),
                                                           this.DialogService.Instance(),
                                                           this.DeviceService.Instance(),
                                                           this.NavigationParameterService.Instance(),
                                                           this.Mediator.Instance());
    }

    [Fact]
    public async Task MyAccountDetailsPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.ViewModel.Balance.ShouldBe(TestData.Balance);
        this.ViewModel.AvailableBalance.ShouldBe(TestData.AvailableBalance);
        this.ViewModel.MerchantName.ShouldBe(TestData.MerchantName);
        this.ViewModel.LastStatementDate.ShouldBe(TestData.LastStatementDate);
        this.ViewModel.NextStatementDate.ShouldBe(TestData.NextStatementDate);
        this.ViewModel.SettlementSchedule.ShouldBe(TestData.SettlementSchedule);
    }

    [Fact]
    public async Task MyAccountDetailsPageViewModel_BackButtonCommand_PreviousPageIsShown()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }
}