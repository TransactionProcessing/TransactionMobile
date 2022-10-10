namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using System;
using System.Collections.Generic;
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

public class VoucherIssueSelectProductPageViewModelTests
{
    #region Methods

    [Fact]
    public async Task VoucherIssueSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied() {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        VoucherSelectProductPageViewModel viewModel = new VoucherSelectProductPageViewModel(mediator.Object, navigationService.Object,applicationCache.Object, dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        viewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
    }

    [Fact]
    public async Task VoucherIssueSelectProductPageViewModel_Initialise_IsInitialised() {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        VoucherSelectProductPageViewModel viewModel = new VoucherSelectProductPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task VoucherIssueSelectProductPageViewModel_ProductSelectedCommand_Execute_IsExecuted() {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        VoucherSelectProductPageViewModel viewModel = new VoucherSelectProductPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel> {
                                                                                                                SelectedItemIndex = 1,
                                                                                                                SelectedItem = TestData.Operator1Product_100KES
                                                                                                            };

        viewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        navigationService.Verify(n => n.GoToVoucherIssueVoucherPage(It.IsAny<ProductDetails>(),It.IsAny<Decimal>()), Times.Once);
    }

    [Fact]
    public async Task VoucherIssueSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        VoucherSelectProductPageViewModel viewModel = new VoucherSelectProductPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoBack(), Times.Once);
    }

    #endregion
}