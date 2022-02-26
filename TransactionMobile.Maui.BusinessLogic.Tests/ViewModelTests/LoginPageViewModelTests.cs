namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System.Threading;
using Maui.UIServices;
using MediatR;
using Moq;
using Requests;
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
        LoginPageViewModel viewModel = new LoginPageViewModel(mediator.Object, navigationService.Object);

        viewModel.LoginCommand.Execute(null);
        mediator.Verify(x => x.Send(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(It.IsAny<LogonTransactionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(x => x.Send(It.IsAny<GetMerchantBalanceRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(n => n.GoToHome(), Times.Once);
    }
}