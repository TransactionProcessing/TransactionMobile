using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions;

[Collection("ViewModelTests")]
public class TransactionsPageViewModelTests
{
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly INavigationServiceImposter NavigationService;
    private INavigationParameterServiceImposter NavigationParameterService;
    private readonly TransactionsPageViewModel ViewModel;
    private readonly IDialogServiceImposter DialogSevice;

    private readonly IDeviceServiceImposter DeviceService;

    public TransactionsPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new TransactionsPageViewModel(this.NavigationService.Instance(), this.ApplicationCache.Instance(), this.DialogSevice.Instance(), this.DeviceService.Instance(),
            this.NavigationParameterService.Instance());
        
    }

    [Fact]
    public void TransactionsPageViewModel_AdminCommand_Execute_IsExecuted()
    {
        this.ViewModel.AdminCommand.Execute(null);
        this.NavigationService.GoToAdminPage().Called(Count.Once());
    }

    [Fact]
    public void TransactionsPageViewModel_BillPaymentCommand_Execute_IsExecuted()
    {
        this.ViewModel.BillPaymentCommand.Execute(null);
        this.NavigationService.GoToBillPaymentSelectOperatorPage().Called(Count.Once());
    }

    [Fact]
    public void TransactionsPageViewModel_MobileTopupCommand_Execute_IsExecuted()
    {
        this.ViewModel.MobileTopupCommand.Execute(null);
        this.NavigationService.GoToMobileTopupSelectOperatorPage().Called(Count.Once());
    }

    [Fact]
    public void TransactionsPageViewModel_MobileWalletCommand_Execute_IsExecuted()
    {
        this.ViewModel.MobileWalletCommand.Execute(null);
        this.NavigationService.GoToHome().Called(Count.Once());
    }

    [Fact]
    public void TransactionsPageViewModel_VoucherCommand_Execute_IsExecuted()
    {
        this.ViewModel.VoucherCommand.Execute(null);
        this.NavigationService.GoToVoucherSelectOperatorPage().Called(Count.Once());
    }

    [Fact]
    public void TransactionsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.GoToHome().Called(Count.Once());
    }
}