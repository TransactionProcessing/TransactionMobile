namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using Maui.UIServices;
using Moq;
using Shared.Logger;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupSuccessPageViewModelTests
{
    [Fact]
    public void MobileTopupSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupSuccessPageViewModel viewModel = new MobileTopupSuccessPageViewModel(navigationService.Object);

        viewModel.CompletedCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}