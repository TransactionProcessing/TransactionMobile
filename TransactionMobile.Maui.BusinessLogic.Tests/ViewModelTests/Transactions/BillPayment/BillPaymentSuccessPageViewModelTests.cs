namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using Maui.UIServices;
using Moq;
using Shared.Logger;
using ViewModels.Transactions;
using Xunit;

public class BillPaymentSuccessPageViewModelTests
{
    [Fact]
    public void BillPaymentSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentSuccessPageViewModel viewModel = new BillPaymentSuccessPageViewModel(navigationService.Object);

        viewModel.CompletedCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}