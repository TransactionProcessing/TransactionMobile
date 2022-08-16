namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System;
using System.Threading;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Moq;
using Requests;
using Shared.Logger;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.Database;
using UIServices;
using ViewModels;
using Xunit;

public class LoginPageViewModelTests
{
    [Fact]
    public void LoginPageViewModel_LoginCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
        Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
        Mock<IDialogService> dialogService = new Mock<IDialogService>();

        LoginPageViewModel viewModel = new LoginPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object,
                                                              deviceService.Object,applicationInfoService.Object,
                                                              dialogService.Object);
        Logger.Initialise(NullLogger.Instance);
        mediator.Setup(m => m.Send(It.IsAny<GetConfigurationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Configuration());
        mediator.Setup(m => m.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.AccessToken);
        mediator.Setup(m => m.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformLogonResponseModel);
        
        viewModel.LoginCommand.Execute(null);
        
        mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }
}