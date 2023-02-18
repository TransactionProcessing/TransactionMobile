namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System;
using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using Xunit;
using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class HomePageViewModelTests
{
    private Mock<INavigationService> navigationService;

    private Mock<IApplicationCache> applicationCache;

    private Mock<IDialogService> dialogService;

    private HomePageViewModel viewModel;

    public HomePageViewModelTests() {
         this.navigationService = new Mock<INavigationService>();
        applicationCache = new Mock<IApplicationCache>();
        dialogService = new Mock<IDialogService>();
        viewModel = new HomePageViewModel(applicationCache.Object,
                                                            dialogService.Object,
                                                            navigationService.Object);
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsToLogout_LoginPageDisplayed()
    {
        dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(true);
        
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
        dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(false);
     
        viewModel.BackButtonCommand.Execute(null);

        navigationService.VerifyNoOtherCalls();
        dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }
}