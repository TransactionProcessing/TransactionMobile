namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System;
using System.Collections.Generic;
using System.Threading;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;
using UIServices;
using ViewModels;
using Xunit;
using NullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger;

[Collection("ViewModelTests")]
public class LoginPageViewModelTests
{
    private LoginPageViewModel ViewModel;

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDeviceService> DeviceService;

    private readonly Mock<IApplicationInfoService> ApplicationInfoService;

    private readonly Mock<IDialogService> DialogService;
    public LoginPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ApplicationInfoService = new Mock<IApplicationInfoService>();
        this.DialogService = new Mock<IDialogService>();

        this.ViewModel = new LoginPageViewModel(this.Mediator.Object, this.NavigationService.Object, this.ApplicationCache.Object,
                                                this.DeviceService.Object, this.ApplicationInfoService.Object,
                                                this.DialogService.Object);
        Logger.Initialise(new Logging.NullLogger());
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<TokenResponseModel>(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<PerformLogonResponseModel>(TestData.PerformLogonResponseModel));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Decimal>(TestData.MerchantBalance));

        this.ViewModel.LoginCommand.Execute(null);
        
        this.Mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
    }

    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_ErrorGettingConfig_WarningToastIsShown()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ErrorResult<Configuration>("Error"));

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
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ErrorResult<TokenResponseModel>("Error"));

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
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<TokenResponseModel>(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ErrorResult<PerformLogonResponseModel>(""));

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
        this.Mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));
        this.Mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<TokenResponseModel>(TestData.AccessToken));
        this.Mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<PerformLogonResponseModel>(TestData.PerformLogonResponseModel));
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ErrorResult<List<ContractProductModel>>(""));

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
}