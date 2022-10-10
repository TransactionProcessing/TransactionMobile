namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using Maui.UIServices;
using Moq;
using Shared.Logger;
using ViewModels.Transactions;
using Xunit;

public class BillPaymentFailedPageViewModelTests
{
    [Fact]
    public void BillPaymentFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();

        Logger.Initialise(NullLogger.Instance);
        BillPaymentFailedPageViewModel viewModel = new BillPaymentFailedPageViewModel(navigationService.Object);

        viewModel.CancelledCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}