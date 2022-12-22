namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System;
using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using Xunit;

public class HomePageViewModelTests
{
    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsToLogout_LoginPageDisplayed()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogService = new Mock<IDialogService>();
        dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(true);
        HomePageViewModel viewModel = new HomePageViewModel(applicationCache.Object,
                                                            dialogService.Object,
                                                            navigationService.Object);
        Logger.Initialise(NullLogger.Instance);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoToLoginPage(), Times.Once);
        dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsNotToLogout_LoginPageDisplayed()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogService = new Mock<IDialogService>();
        dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(false);
        HomePageViewModel viewModel = new HomePageViewModel(applicationCache.Object,
                                                            dialogService.Object,
                                                            navigationService.Object);
        Logger.Initialise(NullLogger.Instance);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.VerifyNoOtherCalls();
        dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }
}