namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions;

using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

public class TransactionsPageViewModelTests
{
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<INavigationService> NavigationService;
    private readonly TransactionsPageViewModel ViewModel;
    private readonly Mock<IDialogService> DialogSevice;
    private readonly Mock<ILoggerService> LoggerService;
    public TransactionsPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.LoggerService= new Mock<ILoggerService>();
        this.ViewModel = new TransactionsPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object, this.DialogSevice.Object,
                                                       this.LoggerService.Object);
        
    }

    [Fact]
    public void TransactionsPageViewModel_AdminCommand_Execute_IsExecuted()
    {
        this.ViewModel.AdminCommand.Execute(null);
        this.NavigationService.Verify(n => n.GoToAdminPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_BillPaymentCommand_Execute_IsExecuted()
    {
        this.ViewModel.BillPaymentCommand.Execute(null);
        this.NavigationService.Verify(n => n.GoToBillPaymentSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_MobileTopupCommand_Execute_IsExecuted()
    {
        this.ViewModel.MobileTopupCommand.Execute(null);
        this.NavigationService.Verify(n => n.GoToMobileTopupSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_MobileWalletCommand_Execute_IsExecuted()
    {
        this.ViewModel.MobileWalletCommand.Execute(null);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_VoucherCommand_Execute_IsExecuted()
    {
        this.ViewModel.VoucherCommand.Execute(null);
        this.NavigationService.Verify(n => n.GoToVoucherSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
    }
}