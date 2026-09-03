using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class HomePageViewModelTests
{
    private INavigationServiceImposter navigationService;
    private INavigationParameterServiceImposter navigationParameterService;

    private IApplicationCacheImposter applicationCache;

    private IDialogServiceImposter dialogService;

    private HomePageViewModel viewModel;

    private readonly IDeviceServiceImposter DeviceService;
    private readonly IBalanceRefresherImposter balanceRefresher;

    public HomePageViewModelTests() {
         this.navigationService = new INavigationServiceImposter();
        this.applicationCache = new IApplicationCacheImposter();
        this.dialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.navigationParameterService = new INavigationParameterServiceImposter();
        this.balanceRefresher = new IBalanceRefresherImposter();
        this.viewModel = new HomePageViewModel(this.applicationCache.Instance(),
                                                            this.dialogService.Instance(),
                                                            this.DeviceService.Instance(),
                                                            this.navigationService.Instance(),
                                                            this.navigationParameterService.Instance(),
                                                            this.balanceRefresher.Instance());
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsToLogout_LoginPageDisplayed()
    {
        this.dialogService.ShowDialog(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any()).ReturnsAsync(true);
        
        this.viewModel.BackButtonCommand.Execute(null);

        this.navigationService.GoToLoginPage().Called(Count.Once());
        this.dialogService.ShowDialog(Arg<String>.Any(),
                                               Arg<String>.Any(),
                                               Arg<String>.Any(),
                                               Arg<String>.Any()).Called(Count.Once());
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsNotToLogout_LoginPageDisplayed()
    {
        this.dialogService.ShowDialog(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any()).ReturnsAsync(false);
     
        this.viewModel.BackButtonCommand.Execute(null);

        this.navigationService.GoToLoginPage().Called(Count.Never());
        this.dialogService.ShowDialog(Arg<String>.Any(),
                                               Arg<String>.Any(),
                                               Arg<String>.Any(),
                                               Arg<String>.Any()).Called(Count.Once());
    }

    [Fact]
    public void HomePageViewModel_GoToTransactionsCommand_Execute_TransactionsPageDisplayed()
    {
        this.viewModel.GoToTransactionsCommand.Execute(null);

        this.navigationService.GoToTransactions().Called(Count.Once());
    }

    [Fact]
    public void HomePageViewModel_GoToMobileTopupCommand_Execute_MobileTopupSelectOperatorPageDisplayed()
    {
        this.viewModel.GoToMobileTopupCommand.Execute(null);

        this.navigationService.GoToMobileTopupSelectOperatorPage().Called(Count.Once());
    }

    [Fact]
    public void HomePageViewModel_GoToBillPaymentCommand_Execute_BillPaymentSelectOperatorPageDisplayed()
    {
        this.viewModel.GoToBillPaymentCommand.Execute(null);

        this.navigationService.GoToBillPaymentSelectOperatorPage().Called(Count.Once());
    }

    [Fact]
    public void HomePageViewModel_GoToVoucherCommand_Execute_VoucherSelectOperatorPageDisplayed()
    {
        this.viewModel.GoToVoucherCommand.Execute(null);

        this.navigationService.GoToVoucherSelectOperatorPage().Called(Count.Once());
    }

    [Fact]
    public void HomePageViewModel_GoToAdminCommand_Execute_AdminPageDisplayed()
    {
        this.viewModel.GoToAdminCommand.Execute(null);

        this.navigationService.GoToAdminPage().Called(Count.Once());
    }

    [Fact]
    public async Task HomePageViewModel_Initialise_Balance_SetsBalanceFromCache()
    {
        this.applicationCache.GetMerchantBalance().Returns(123.45m);

        await this.viewModel.Initialise(CancellationToken.None);

        Assert.Equal("Balance: 123.45", this.viewModel.Balance);
    }

    [Fact]
    public void HomePageViewModel_BalanceChanged_Updates_Balance()
    {
        Action<Decimal> capturedHandler = null;
        this.balanceRefresher.BalanceChanged.OnSubscribe(h => capturedHandler = h);

        var vm = new HomePageViewModel(this.applicationCache.Instance(),
                                       this.dialogService.Instance(),
                                       this.DeviceService.Instance(),
                                       this.navigationService.Instance(),
                                       this.navigationParameterService.Instance(),
                                       this.balanceRefresher.Instance());

        capturedHandler?.Invoke(200.00m);

        Assert.Equal("Balance: 200.00", vm.Balance);
    }
}
