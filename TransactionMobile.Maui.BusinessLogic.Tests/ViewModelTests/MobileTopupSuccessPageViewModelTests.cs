namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Moq;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupSuccessPageViewModelTests
{
    [Fact]
    public void MobileTopupSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        MobileTopupSuccessPageViewModel viewModel = new MobileTopupSuccessPageViewModel(navigationService.Object);

        viewModel.CompletedCommand.Execute(null);
        navigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}