using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests;

[Collection("ViewModelTests")]
public class LoginPageViewModelTests
{
    private LoginPageViewModel ViewModel;

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;
    private Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDeviceService> DeviceService;

    private readonly Mock<IApplicationInfoService> ApplicationInfoService;

    private readonly Mock<IApplicationUpdateLauncherService> ApplicationUpdateLauncherService;

    private readonly Mock<IDialogService> DialogService;

    private readonly Mock<IUpdateService> UpdateService;
    public LoginPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ApplicationInfoService = new Mock<IApplicationInfoService>();
        this.ApplicationUpdateLauncherService = new Mock<IApplicationUpdateLauncherService>();
        this.DialogService = new Mock<IDialogService>();
        this.UpdateService = new Mock<IUpdateService>();

        this.ViewModel = new LoginPageViewModel(this.Mediator.Object, this.NavigationService.Object, this.ApplicationCache.Object,
                                                this.DeviceService.Object, this.ApplicationInfoService.Object,
                                                this.DialogService.Object, this.NavigationParameterService.Object,
                                                this.UpdateService.Object, this.ApplicationUpdateLauncherService.Object);
        Logger.Initialise(new Logging.NullLogger());
    }
    
    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantBalance));

        this.ViewModel.LogonCommand.Execute(null);
        
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("http://localhost")]
    public void LoginPageViewModel_LoginCommand_Execute_ConfigUrlSet_IsExecuted(String configUrl)
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantBalance));
        this.ViewModel.ConfigHostUrl = configUrl;

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        if (String.IsNullOrEmpty(configUrl) == false){
            this.ApplicationCache.Verify(v => v.SetConfigHostUrl(It.IsAny<String>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
        }
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorGettingConfig_WarningToastIsShown()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure("Error"));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Verify(x => x.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Never);

        this.DialogService.Verify(n => n.ShowWarningToast(It.IsAny<String>(),
                                                          null,
                                                          "OK",
                                                          null,
                                                          CancellationToken.None), Times.Once);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorGettingToken_WarningToastIsShown()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure("Error"));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Verify(x => x.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Never);

        this.DialogService.Verify(n => n.ShowWarningToast(It.IsAny<String>(),
                                                          null,
                                                          "OK",
                                                          null,
                                                          CancellationToken.None), Times.Once);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_UpdateCheckFails_LogonContinues()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new Configuration { EnableAutoUpdates = true }));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantBalance));
        this.ApplicationInfoService.Setup(a => a.VersionString).Returns(TestData.ApplicationVersion);
        this.ApplicationInfoService.Setup(a => a.PackageName).Returns("com.transactionprocessor.mobile");
        this.DeviceService.Setup(d => d.GetPlatform()).Returns("Android");
        this.DeviceService.Setup(d => d.GetIdentifier()).Returns(TestData.DeviceIdentifier);
        this.UpdateService.Setup(u => u.CheckForUpdates(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure("Update check failed"));

        this.ViewModel.LogonCommand.Execute(null);

        this.UpdateService.Verify(u => u.CheckForUpdates(TestData.ApplicationVersion,
                                                         "com.transactionprocessor.mobile",
                                                         "Android",
                                                         TestData.DeviceIdentifier,
                                                         It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_UpdateRequired_UpdateLauncherIsCalled_And_AppQuits()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new Configuration { EnableAutoUpdates = true }));
        this.ApplicationInfoService.Setup(a => a.VersionString).Returns(TestData.ApplicationVersion);
        this.ApplicationInfoService.Setup(a => a.PackageName).Returns("com.transactionprocessor.mobile");
        this.DeviceService.Setup(d => d.GetPlatform()).Returns("Android");
        this.DeviceService.Setup(d => d.GetIdentifier()).Returns(TestData.DeviceIdentifier);
        this.UpdateService.Setup(u => u.CheckForUpdates(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ApplicationUpdateCheckResponse
            {
                DownloadUri = "https://updates.example.com/transactionmobile.apk",
                LatestVersion = "1.0.1",
                Message = "Install update",
                UpdateRequired = true
            }));
        this.DialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(true);

        this.ViewModel.LogonCommand.Execute(null);

        this.DialogService.Verify(d => d.ShowInformationToast("Downloading the required update...",
                                                              null,
                                                              "OK",
                                                              null,
                                                              CancellationToken.None), Times.Once);
        this.ApplicationUpdateLauncherService.Verify(l => l.LaunchUpdateAsync("https://updates.example.com/transactionmobile.apk", It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(n => n.QuitApplication(), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Never);
        this.DialogService.Verify(n => n.ShowWarningToast(It.IsAny<String>(),
                                                          null,
                                                          "OK",
                                                          null,
                                                          CancellationToken.None), Times.Never);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_UpdateLauncherFails_WarningToastIsShown_And_AppStaysOpen()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new Configuration { EnableAutoUpdates = true }));
        this.ApplicationInfoService.Setup(a => a.VersionString).Returns(TestData.ApplicationVersion);
        this.ApplicationInfoService.Setup(a => a.PackageName).Returns("com.transactionprocessor.mobile");
        this.DeviceService.Setup(d => d.GetPlatform()).Returns("Android");
        this.DeviceService.Setup(d => d.GetIdentifier()).Returns(TestData.DeviceIdentifier);
        this.UpdateService.Setup(u => u.CheckForUpdates(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ApplicationUpdateCheckResponse
            {
                DownloadUri = "https://updates.example.com/transactionmobile.apk",
                LatestVersion = "1.0.1",
                Message = "Install update",
                UpdateRequired = true
            }));
        this.DialogService.Setup(d => d.ShowDialog(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).ReturnsAsync(true);
        this.ApplicationUpdateLauncherService.Setup(l => l.LaunchUpdateAsync(It.IsAny<String>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApplicationException("Unable to start the application update installer."));

        this.ViewModel.LogonCommand.Execute(null);

        this.NavigationService.Verify(n => n.QuitApplication(), Times.Never);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Never);
        this.DialogService.Verify(d => d.ShowWarningToast("Unable to start the application update installer.",
                                                          null,
                                                          "OK",
                                                          null,
                                                          CancellationToken.None), Times.Once);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorDuringLogonTransaction_WarningToastIsShown()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure(""));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Verify(x => x.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Never);

        this.DialogService.Verify(n => n.ShowWarningToast(It.IsAny<String>(),
                                                          null,
                                                          "OK",
                                                          null,
                                                          CancellationToken.None), Times.Once);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorDuringGetContractProducts_WarningToastIsShown()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure(""));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Verify(x => x.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Never);

        this.DialogService.Verify(n => n.ShowWarningToast(It.IsAny<String>(),
                                                     null,
                                                     "OK",
                                                     null,
                                                     CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public void LoginPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.Verify(n => n.QuitApplication(), Times.Once);
    }
    
    [Fact]
    public void LoginPageViewModel_PropertyTests_ValuesAreAsExpected(){
        this.DeviceService.Setup(d => d.GetIdentifier()).Returns("testidentifier");
        
        this.ViewModel.Password = TestData.Password;
        this.ViewModel.UserName = TestData.UserName;
        this.ViewModel.UseTrainingMode = true;
        this.ViewModel.ConfigHostUrl = "http://localhost";
        
        this.ViewModel.UserName.ShouldBe(TestData.UserName);
        this.ViewModel.Password.ShouldBe(TestData.Password);
        this.ViewModel.ConfigHostUrl.ShouldBe("http://localhost");
        this.ViewModel.UseTrainingMode.ShouldBeTrue();
        this.ViewModel.DeviceIdentifier.ShouldBe("testidentifier");
    }
}
