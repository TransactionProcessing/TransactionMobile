namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Moq;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupFailedViewModelTests
{
    [Fact]
    public void MobileTopupFailedPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        MobileTopupFailedPageViewModel viewModel = new MobileTopupFailedPageViewModel(navigationService.Object);

        viewModel.CancelledCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}