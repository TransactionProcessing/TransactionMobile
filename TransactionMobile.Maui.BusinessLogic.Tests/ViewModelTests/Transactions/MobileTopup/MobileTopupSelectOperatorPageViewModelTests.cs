namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using System.Threading;
using System.Threading.Tasks;
using Common;
using Maui.UIServices;
using MediatR;
using Models;
using Moq;
using Requests;
using Services;
using Shared.Logger;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupSelectOperatorPageViewModelTests
{
    [Fact]
    public async Task MobileTopupSelectOperatorPageViewModel_Initialise_IsInitialised()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        MobileTopupSelectOperatorPageViewModel viewModel = new MobileTopupSelectOperatorPageViewModel(mediator.Object, navigationService.Object,dialogSevice.Object,applicationCache.Object);

        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Operators.Count.ShouldBe(3);
    }

    [Fact]
    public async Task MobileTopupSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupSelectOperatorPageViewModel viewModel = new MobileTopupSelectOperatorPageViewModel(mediator.Object, navigationService.Object,dialogSevice.Object,applicationCache.Object);

        await viewModel.Initialise(CancellationToken.None);

        viewModel.Operators.Count.ShouldBe(3);

        ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
        {
            SelectedItemIndex = 1,
            SelectedItem = TestData.ContractOperatorModel
        };

        viewModel.OperatorSelectedCommand.Execute(selectedContractOperator);
        
        navigationService.Verify(n => n.GoToMobileTopupSelectProductPage(It.IsAny<ProductDetails>()), Times.Once);
    }

    [Fact]
    public async Task MobileTopupSelectOperatorPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupSelectOperatorPageViewModel viewModel = new MobileTopupSelectOperatorPageViewModel(mediator.Object, navigationService.Object, dialogSevice.Object, applicationCache.Object);

        ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
                                                                       {
                                                                           SelectedItemIndex = 1,
                                                                           SelectedItem = TestData.ContractOperatorModel
                                                                       };

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoBack(), Times.Once);
    }
}