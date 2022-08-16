namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using Maui.UIServices;
using Moq;
using Services;
using ViewModels.MyAccount;
using Xunit;

public class MyAccountPageViewModelTests
{
    [Fact]
    public void MyAccountPageViewModel_LogoutCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        MyAccountPageViewModel viewModel = new MyAccountPageViewModel(navigationService.Object,
                                                                      applicationCache.Object);

        viewModel.LogoutCommand.Execute(null);
        navigationService.Verify(n => n.GoToLoginPage(), Times.Once);
    }
}