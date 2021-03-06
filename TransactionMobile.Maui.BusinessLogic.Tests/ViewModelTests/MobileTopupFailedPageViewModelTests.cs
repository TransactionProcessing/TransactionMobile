namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Maui.UIServices;
using Moq;
using Shared.Logger;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupFailedPageViewModelTests
{
    [Fact]
    public void MobileTopupFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupFailedPageViewModel viewModel = new MobileTopupFailedPageViewModel(navigationService.Object);

        viewModel.CancelledCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}