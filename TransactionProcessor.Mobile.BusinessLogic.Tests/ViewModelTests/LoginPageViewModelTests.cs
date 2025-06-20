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

    private readonly Mock<IDialogService> DialogService;
    public LoginPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ApplicationInfoService = new Mock<IApplicationInfoService>();
        this.DialogService = new Mock<IDialogService>();

        this.ViewModel = new LoginPageViewModel(this.Mediator.Object, this.NavigationService.Object, this.ApplicationCache.Object,
                                                this.DeviceService.Object, this.ApplicationInfoService.Object,
                                                this.DialogService.Object, this.NavigationParameterService.Object);
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

        this.ViewModel.LoginCommand.Execute(null);
        
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
        this.ViewModel.LoginCommand.Execute(null);

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

        this.ViewModel.LoginCommand.Execute(null);

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

        this.ViewModel.LoginCommand.Execute(null);

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
    public void LoginPageViewModel_LoginCommand_Execute_ErrorDuringLogonTransaction_WarningToastIsShown()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure(""));

        this.ViewModel.LoginCommand.Execute(null);

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

        this.ViewModel.LoginCommand.Execute(null);

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