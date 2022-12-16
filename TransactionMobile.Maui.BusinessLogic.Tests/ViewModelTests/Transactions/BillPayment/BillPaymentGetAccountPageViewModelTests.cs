﻿namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Maui.UIServices;
using MediatR;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shared.Logger;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class BillPaymentGetAccountPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentGetAccountPageViewModel ViewModel;

    public BillPaymentGetAccountPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.ViewModel = new BillPaymentGetAccountPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object, this.DialogSevice.Object, this.Mediator.Object);
        Logger.Initialise(NullLogger.Instance);
    }
    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));
        
        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.Operator1ProductDetails.OperatorIdentifier);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<PerformBillPaymentGetAccountResponseModel>(TestData.PerformBillPaymentGetAccountResponseModel));
        
        BillPaymentGetAccountPageViewModel viewModel = new BillPaymentGetAccountPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object, this.DialogSevice.Object, this.Mediator.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        viewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;
        
        viewModel.GetAccountCommand.Execute(null);
        
        this.NavigationService.Verify(n => n.GoToBillPaymentPayBillPage(It.IsAny<ProductDetails>(), It.IsAny<BillDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Failed_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<PerformBillPaymentGetAccountResponseModel>(TestData.PerformBillPaymentGetAccountResponseModelFailed));
        
        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        this.ViewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;

        this.ViewModel.GetAccountCommand.Execute(null);
        
        this.NavigationService.Verify(n => n.GoToBillPaymentFailedPage(), Times.Once);
    }


    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        //this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformBillPaymentGetAccountResponseModelFailed);
        
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}