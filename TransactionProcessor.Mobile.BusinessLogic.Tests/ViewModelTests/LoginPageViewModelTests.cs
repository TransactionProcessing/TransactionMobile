using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Imposter.Abstractions;
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

    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;
    private INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDeviceServiceImposter DeviceService;

    private readonly IApplicationInfoServiceImposter ApplicationInfoService;

    private readonly IApplicationUpdateLauncherServiceImposter ApplicationUpdateLauncherService;

    private readonly IDialogServiceImposter DialogService;

    private readonly IUpdateServiceImposter UpdateService;

    private readonly IBalanceRefresherImposter BalanceRefresher;

    private readonly ISentryServiceImposter SentryService;

    private int warningToastCount;
    private int informationToastCount;

    public LoginPageViewModelTests() {
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ApplicationInfoService = new IApplicationInfoServiceImposter();
        this.ApplicationUpdateLauncherService = new IApplicationUpdateLauncherServiceImposter();
        this.DialogService = new IDialogServiceImposter();
        this.UpdateService = new IUpdateServiceImposter();
        this.BalanceRefresher = new IBalanceRefresherImposter();
        this.SentryService = new ISentryServiceImposter();
        this.DialogService.ShowWarningToast(Arg<String>.Any(), Arg<Action?>.Any(), Arg<String?>.Any(), Arg<TimeSpan?>.Any(), Arg<CancellationToken>.Any())
            .Returns(Task.CompletedTask)
            .Callback((String _, Action? _, String? _, TimeSpan? _, CancellationToken _) =>
            {
                this.warningToastCount++;
                return Task.CompletedTask;
            });
        this.DialogService.ShowInformationToast(Arg<String>.Any(), Arg<Action?>.Any(), Arg<String?>.Any(), Arg<TimeSpan?>.Any(), Arg<CancellationToken>.Any())
            .Returns(Task.CompletedTask)
            .Callback((String _, Action? _, String? _, TimeSpan? _, CancellationToken _) =>
            {
                this.informationToastCount++;
                return Task.CompletedTask;
            });

        this.ViewModel = new LoginPageViewModel(this.Mediator.Instance(), this.NavigationService.Instance(), this.ApplicationCache.Instance(),
                                                this.DeviceService.Instance(), this.ApplicationInfoService.Instance(),
                                                this.DialogService.Instance(), this.NavigationParameterService.Instance(),
                                                this.UpdateService.Instance(), this.ApplicationUpdateLauncherService.Instance(),
                                                this.BalanceRefresher.Instance(), this.SentryService.Instance());
        Logger.Initialise(new Logging.NullLogger());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("http://localhost")]
    public void LoginPageViewModel_LoginCommand_Execute_ConfigUrlSet_IsExecuted(String configUrl)
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        this.ViewModel.ConfigHostUrl = configUrl;

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToHome().Called(Count.Once());
        if (String.IsNullOrEmpty(configUrl) == false){
            this.ApplicationCache.SetConfigHostUrl(Arg<String>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Once());
        }
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorGettingConfig_WarningToastIsShown()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Failure("Error"));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        this.NavigationService.GoToHome().Called(Count.Never());

        this.warningToastCount.ShouldBe(1);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorGettingToken_WarningToastIsShown()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Failure("Error"));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        this.NavigationService.GoToHome().Called(Count.Never());

        this.warningToastCount.ShouldBe(1);
    }

    [Fact]
    public async Task LoginPageViewModel_LoginCommand_Execute_UpdateCheckFails_LogonContinues()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new Configuration { EnableAutoUpdates = true }));
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.Mediator.Send(Arg<IRequest<Result<Decimal>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantBalance));
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));
        this.Mediator.Send(Arg<IRequest<Result<MerchantDetailsModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        this.ApplicationInfoService.VersionString.Getter().Returns(TestData.ApplicationVersion);
        this.ApplicationInfoService.PackageName.Getter().Returns("com.transactionprocessor.mobile");
        this.DeviceService.GetPlatform().Returns("Android");
        this.DeviceService.GetIdentifier().Returns(TestData.DeviceIdentifier);
        this.UpdateService.CheckForUpdates(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Failure("Update check failed"));

        await this.ViewModel.LogonCommand.ExecuteAsync(null);

        this.UpdateService.CheckForUpdates(TestData.ApplicationVersion,
                                                         "com.transactionprocessor.mobile",
                                                         "Android",
                                                         TestData.DeviceIdentifier,
                                                         Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToHome().Called(Count.Once());
    }

    [Fact]
    public async Task LoginPageViewModel_LoginCommand_Execute_UpdateRequired_UpdateLauncherIsCalled_And_AppQuits()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new Configuration { EnableAutoUpdates = true }));
        this.ApplicationInfoService.VersionString.Getter().Returns(TestData.ApplicationVersion);
        this.ApplicationInfoService.PackageName.Getter().Returns("com.transactionprocessor.mobile");
        this.DeviceService.GetPlatform().Returns("Android");
        this.DeviceService.GetIdentifier().Returns(TestData.DeviceIdentifier);
        this.UpdateService.CheckForUpdates(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new ApplicationUpdateCheckResponse
            {
                DownloadUri = "https://updates.example.com/transactionmobile.apk",
                LatestVersion = "1.0.1",
                Message = "Install update",
                UpdateRequired = true
            }));
        this.DialogService.ShowDialog(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any()).ReturnsAsync(true);

        await this.ViewModel.LogonCommand.ExecuteAsync(null);

        this.informationToastCount.ShouldBe(1);
        this.ApplicationUpdateLauncherService.LaunchUpdateAsync("https://updates.example.com/transactionmobile.apk", Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.QuitApplication().Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        this.NavigationService.GoToHome().Called(Count.Never());
        this.warningToastCount.ShouldBe(0);
    }

    [Fact]
    public async Task LoginPageViewModel_LoginCommand_Execute_UpdateLauncherFails_WarningToastIsShown_And_AppStaysOpen()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new Configuration { EnableAutoUpdates = true }));
        this.ApplicationInfoService.VersionString.Getter().Returns(TestData.ApplicationVersion);
        this.ApplicationInfoService.PackageName.Getter().Returns("com.transactionprocessor.mobile");
        this.DeviceService.GetPlatform().Returns("Android");
        this.DeviceService.GetIdentifier().Returns(TestData.DeviceIdentifier);
        this.UpdateService.CheckForUpdates(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new ApplicationUpdateCheckResponse
            {
                DownloadUri = "https://updates.example.com/transactionmobile.apk",
                LatestVersion = "1.0.1",
                Message = "Install update",
                UpdateRequired = true
            }));
        this.DialogService.ShowDialog(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any()).ReturnsAsync(true);
        this.ApplicationUpdateLauncherService.LaunchUpdateAsync(Arg<String>.Any(), Arg<CancellationToken>.Any())
            .ThrowsAsync(new ApplicationException("Unable to start the application update installer."));

        await this.ViewModel.LogonCommand.ExecuteAsync(null);

        this.NavigationService.QuitApplication().Called(Count.Never());
        this.NavigationService.GoToHome().Called(Count.Never());
        this.warningToastCount.ShouldBe(1);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorDuringLogonTransaction_WarningToastIsShown()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Failure(""));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        
        this.NavigationService.GoToHome().Called(Count.Never());

        this.warningToastCount.ShouldBe(1);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorDuringGetContractProducts_WarningToastIsShown()
    {
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new Configuration()));
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Failure(""));

        this.ViewModel.LogonCommand.Execute(null);

        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToHome().Called(Count.Never());

        this.warningToastCount.ShouldBe(1);
    }
    
    [Fact]
    public void LoginPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.QuitApplication().Called(Count.Once());
    }

    [Fact]
    public async Task LoginPageViewModel_LoginCommand_Execute_SentryInitialisedWithDsnFromConfiguration()
    {
        String sentryDsn = "https://key@sentry.io/123";
        this.Mediator.Send(Arg<IRequest<Result<Configuration>>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new Configuration { SentryDsn = sentryDsn }));
        this.Mediator.Send(Arg<IRequest<Result<TokenResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));
        this.Mediator.Send(Arg<IRequest<Result<PerformLogonResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.PerformLogonResponseModel));
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        await this.ViewModel.LogonCommand.ExecuteAsync(null);

        this.SentryService.InitializeSentry(sentryDsn).Called(Count.Once());
    }
    
    [Fact]
    public void LoginPageViewModel_PropertyTests_ValuesAreAsExpected(){
        this.DeviceService.GetIdentifier().Returns("testidentifier");
        
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
