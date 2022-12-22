namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class BillPaymentSelectProductPageViewModelTests
{
    #region Methods

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentSelectProductPageViewModel ViewModel;
    private readonly Mock<ILoggerService> LoggerService;
    public BillPaymentSelectProductPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.LoggerService = new Mock<ILoggerService>();
        this.ViewModel = new BillPaymentSelectProductPageViewModel(this.Mediator.Object,
                                                                   this.NavigationService.Object,
                                                                   this.ApplicationCache.Object,
                                                                   this.DialogSevice.Object,
                                                                   this.LoggerService.Object);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        this.ViewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ProductSelectedCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        this.ViewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_100KES
                                                                     };

        this.ViewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        this.NavigationService.Verify(n => n.GoToBillPaymentGetAccountPage(It.IsAny<ProductDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }

    #endregion
}