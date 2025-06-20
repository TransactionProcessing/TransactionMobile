using Moq;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class HomePageViewModelTests
{
    private Mock<INavigationService> navigationService;
    private Mock<INavigationParameterService> navigationParameterService;

    private Mock<IApplicationCache> applicationCache;

    private Mock<IDialogService> dialogService;

    private HomePageViewModel viewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public HomePageViewModelTests() {
         this.navigationService = new Mock<INavigationService>();
        this.applicationCache = new Mock<IApplicationCache>();
        this.dialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.navigationParameterService = new Mock<INavigationParameterService>();
        this.viewModel = new HomePageViewModel(this.applicationCache.Object,
                                                            this.dialogService.Object,
                                                            this.DeviceService.Object,
                                                            this.navigationService.Object,
                                                            this.navigationParameterService.Object);
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsToLogout_LoginPageDisplayed()
    {
        this.dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(true);
        
        this.viewModel.BackButtonCommand.Execute(null);

        this.navigationService.Verify(n => n.GoToLoginPage(), Times.Once);
        this.dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }

    [Fact]
    public void HomePageViewModel_BackButtonCommand_Execute_UserSelectsNotToLogout_LoginPageDisplayed()
    {
        this.dialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(false);
     
        this.viewModel.BackButtonCommand.Execute(null);

        this.navigationService.VerifyNoOtherCalls();
        this.dialogService.Verify(d => d.ShowDialog(It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>(),
                                               It.IsAny<String>()), Times.Once);
    }
}