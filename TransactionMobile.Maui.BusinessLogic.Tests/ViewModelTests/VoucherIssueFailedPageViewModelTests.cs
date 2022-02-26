namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Maui.UIServices;
using Moq;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherIssueFailedPageViewModelTests
{
    [Fact]
    public void VoucherIssueFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherIssueFailedPageViewModel viewModel = new VoucherIssueFailedPageViewModel(navigationService.Object);

        viewModel.CancelledCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}