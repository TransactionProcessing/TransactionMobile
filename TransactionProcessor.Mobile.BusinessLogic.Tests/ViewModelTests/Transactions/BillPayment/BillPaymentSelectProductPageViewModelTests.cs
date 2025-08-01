﻿using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using ProductDetails = TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions.ProductDetails;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

[Collection("ViewModelTests")]
public class BillPaymentSelectProductPageViewModelTests
{
    #region Methods

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentSelectProductPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public BillPaymentSelectProductPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new BillPaymentSelectProductPageViewModel(this.Mediator.Object,
                                                                   this.NavigationService.Object,
                                                                   this.ApplicationCache.Object,
                                                                   this.DialogSevice.Object,
                                                                   this.DeviceService.Object,
                                                                   this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.OperatorId1);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        this.ViewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ProductSelectedCommand_PostPay_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        this.ViewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_BillPayment_PostPayment,
                                                                     };

        this.ViewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        this.NavigationService.Verify(n => n.GoToBillPaymentGetAccountPage(It.IsAny<ProductDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ProductSelectedCommand_PrePay_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        this.ViewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_BillPayment_PrePayment,
                                                                     };

        this.ViewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        this.NavigationService.Verify(n => n.GoToBillPaymentGetMeterPage(It.IsAny<ProductDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }

    #endregion
}