namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions;

using Maui.UIServices;
using Moq;
using Services;
using Shared.Logger;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class TransactionsPageViewModelTests
{
    [Fact]
    public void TransactionsPageViewModel_AdminCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object,applicationCache.Object,dialogSevice.Object);

        viewModel.AdminCommand.Execute(null);
        navigationService.Verify(n => n.GoToAdminPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_BillPaymentCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.BillPaymentCommand.Execute(null);
        navigationService.Verify(n => n.GoToBillPaymentSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_MobileTopupCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.MobileTopupCommand.Execute(null);
        navigationService.Verify(n => n.GoToMobileTopupSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_MobileWalletCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.MobileWalletCommand.Execute(null);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_VoucherCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.VoucherCommand.Execute(null);
        navigationService.Verify(n => n.GoToVoucherSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.BackButtonCommand.Execute(null);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }
}