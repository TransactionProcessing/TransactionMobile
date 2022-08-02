namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System.Threading;
using System.Threading.Tasks;
using Common;
using Maui.UIServices;
using MediatR;
using Models;
using Moq;
using Requests;
using Shared.Logger;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherSelectOperatorPageViewModelTests
{
    [Fact]
    public async Task VoucherSelectOperatorPageViewModel_Initialise_IsInitialised()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherSelectOperatorPageViewModel viewModel = new VoucherSelectOperatorPageViewModel(mediator.Object, navigationService.Object);

        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Operators.Count.ShouldBe(3);
    }

    [Fact]
    public async Task VoucherSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        VoucherSelectOperatorPageViewModel viewModel = new VoucherSelectOperatorPageViewModel(mediator.Object, navigationService.Object);

        await viewModel.Initialise(CancellationToken.None);

        viewModel.Operators.Count.ShouldBe(3);

        ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
                                                                       {
                                                                           SelectedItemIndex = 1,
                                                                           SelectedItem = TestData.ContractOperatorModel
                                                                       };

        viewModel.OperatorSelectedCommand.Execute(selectedContractOperator);

        navigationService.Verify(n => n.GoToVoucherSelectProductPage(TestData.OperatorIdentifier1), Times.Once);
    }
}