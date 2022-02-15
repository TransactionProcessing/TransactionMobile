namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Models;
using Moq;
using Requests;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupSelectProductPageViewModelTests
{
    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        MobileTopupSelectProductPageViewModel viewModel = new MobileTopupSelectProductPageViewModel(mediator.Object, navigationService.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1}
                                       });
        viewModel.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_Initialise_IsInitialised()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        MobileTopupSelectProductPageViewModel viewModel = new MobileTopupSelectProductPageViewModel(mediator.Object, navigationService.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1}
                                       });
        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_ProductSelectedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        MobileTopupSelectProductPageViewModel viewModel = new MobileTopupSelectProductPageViewModel(mediator.Object, navigationService.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1}
                                       });
        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_100KES
                                                                     };

        viewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        navigationService.Verify(n=> n.GoToMobileTopupPerformTopupPage(It.IsAny<String>(), It.IsAny<Guid>(),
                                                                       It.IsAny<Guid>(),It.IsAny<Decimal>()), Times.Once);

    }
}