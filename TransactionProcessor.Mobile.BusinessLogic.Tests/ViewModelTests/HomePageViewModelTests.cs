using Moq;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class HomePageViewModelTests
{
    private Mock<INavigationService> navigationService;
    private Mock<INavigationParameterService> navigationParameterService;

    private Mock<IApplicationCache> applicationCache;

    private Mock<IDialogService> dialogService;

    private HomePageViewModel viewModel;

    private readonly Mock<IDeviceService> DeviceService;
    private readonly Mock<IBalanceRefresher> balanceRefresher;

    public HomePageViewModelTests() {
         this.navigationService = new Mock<INavigationService>();
        this.applicationCache = new Mock<IApplicationCache>();
        this.dialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.navigationParameterService = new Mock<INavigationParameterService>();
        this.balanceRefresher = new Mock<IBalanceRefresher>();
        this.balanceRefresher.SetupAdd(b => b.BalanceChanged += It.IsAny<Action<Decimal>>());
        this.viewModel = new HomePageViewModel(this.applicationCache.Object,
                                                            this.dialogService.Object,
                                                            this.DeviceService.Object,
                                                            this.navigationService.Object,
                                                            this.navigationParameterService.Object,
                                                            this.balanceRefresher.Object);
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsToLogout_LoginPageDisplayed()
    {
        this.dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(true);
        
        this.viewModel.BackButtonCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToLoginPage(), Times.Once);
        this.dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsNotToLogout_LoginPageDisplayed()
    {
        this.dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(false);
     
        this.viewModel.BackButtonCommand.Execute(null);

        this.navigationService.VerifyNoOtherCalls();
        this.dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_GoToTransactionsCommand_Execute_TransactionsPageDisplayed()
    {
        this.viewModel.GoToTransactionsCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToTransactions(), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_GoToMobileTopupCommand_Execute_MobileTopupSelectOperatorPageDisplayed()
    {
        this.viewModel.GoToMobileTopupCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToMobileTopupSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_GoToBillPaymentCommand_Execute_BillPaymentSelectOperatorPageDisplayed()
    {
        this.viewModel.GoToBillPaymentCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToBillPaymentSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_GoToVoucherCommand_Execute_VoucherSelectOperatorPageDisplayed()
    {
        this.viewModel.GoToVoucherCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToVoucherSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_GoToAdminCommand_Execute_AdminPageDisplayed()
    {
        this.viewModel.GoToAdminCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToAdminPage(), Times.Once);
    }

    [Fact]
    public async Task HomePageViewModel_Initialise_Balance_SetsBalanceFromCache()
    {
        this.applicationCache.Setup(a => a.GetMerchantBalance()).Returns(123.45m);

        await this.viewModel.Initialise(CancellationToken.None);

        Assert.Equal("Balance: 123.45", this.viewModel.Balance);
    }

    [Fact]
    public void HomePageViewModel_BalanceChanged_Updates_Balance()
    {
        Action<Decimal> capturedHandler = null;
        this.balanceRefresher.SetupAdd(b => b.BalanceChanged += It.IsAny<Action<Decimal>>())
                             .Callback<Action<Decimal>>(h => capturedHandler = h);

        var vm = new HomePageViewModel(this.applicationCache.Object,
                                       this.dialogService.Object,
                                       this.DeviceService.Object,
                                       this.navigationService.Object,
                                       this.navigationParameterService.Object,
                                       this.balanceRefresher.Object);

        capturedHandler?.Invoke(200.00m);

        Assert.Equal("Balance: 200.00", vm.Balance);
    }
}