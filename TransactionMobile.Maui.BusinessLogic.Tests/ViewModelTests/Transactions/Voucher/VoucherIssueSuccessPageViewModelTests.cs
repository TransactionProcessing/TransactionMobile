namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using Maui.UIServices;
using Moq;
using Shared.Logger;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherIssueSuccessPageViewModelTests
{
    [Fact]
    public void VoucherIssueSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        VoucherIssueSuccessPageViewModel viewModel = new VoucherIssueSuccessPageViewModel(navigationService.Object);

        viewModel.CompletedCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}