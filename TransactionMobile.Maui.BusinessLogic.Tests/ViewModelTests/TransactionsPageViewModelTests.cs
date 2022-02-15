namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Moq;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class TransactionsPageViewModelTests
{
    [Fact]
    public void TransactionsPageViewModel_AdminCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object);
            
        viewModel.AdminCommand.Execute(null);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_BillPaymentCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object);

        viewModel.BillPaymentCommand.Execute(null);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_MobileTopupCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object);

        viewModel.MobileTopupCommand.Execute(null);
        navigationService.Verify(n => n.GoToMobileTopupSelectOperatorPage(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_MobileWalletCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object);

        viewModel.MobileWalletCommand.Execute(null);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void TransactionsPageViewModel_VoucherCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        TransactionsPageViewModel viewModel = new TransactionsPageViewModel(navigationService.Object);

        viewModel.VoucherCommand.Execute(null);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }
}