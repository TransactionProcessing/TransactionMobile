namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Maui.UIServices;
using Moq;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherIssueSuccessPageViewModelTests
{
    [Fact]
    public void VoucherIssueSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherIssueSuccessPageViewModel viewModel = new VoucherIssueSuccessPageViewModel(navigationService.Object);

        viewModel.CompletedCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}