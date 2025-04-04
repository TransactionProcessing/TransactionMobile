﻿namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using SimpleResults;
using TransactionProcessorACL.DataTransferObjects.Responses;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

[Collection("ViewModelTests")]
public class BillPaymentPayBillPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentPayBillPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public BillPaymentPayBillPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new BillPaymentPayBillPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object, this.DialogSevice.Object, this.Mediator.Object, this.DeviceService.Object);
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_ApplyQueryAttributes_PostPay_QueryAttributesApplied()
    {
        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                               {nameof(BillDetails), TestData.BillDetails}
                                                                           });

        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.Operator1ProductDetails.OperatorIdentifier);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
        this.ViewModel.BillDetails.AccountName.ShouldBe(TestData.BillDetails.AccountName);
        this.ViewModel.BillDetails.AccountNumber.ShouldBe(TestData.BillDetails.AccountNumber);
        this.ViewModel.BillDetails.Balance.ShouldBe(TestData.BillDetails.Balance);
        this.ViewModel.BillDetails.DueDate.ShouldBe(TestData.BillDetails.DueDate);
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_ApplyQueryAttributes_PrePay_QueryAttributesApplied()
    {
        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                               {nameof(MeterDetails), TestData.MeterDetails}
                                                                           });

        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.Operator1ProductDetails.OperatorIdentifier);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
        this.ViewModel.MeterDetails.MeterNumber.ShouldBe(TestData.MeterDetails.MeterNumber);
        this.ViewModel.MeterDetails.CustomerName.ShouldBe(TestData.MeterDetails.CustomerName);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_CustomerMobileNumberEntryCompleted_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnCustomerMobileNumberEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(BillDetails), TestData.BillDetails}
                                            });
        this.ViewModel.CustomerMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_PaymentAmountEntryCompleted_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnPaymentAmountEntryCompleted = () =>
                                                       {
                                                           isCompletedCalled = true;
                                                       };

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(BillDetails), TestData.BillDetails}
                                            });
        this.ViewModel.PaymentAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_SuccessfulPostPayPayment_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentMakePostPaymentRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel()
                                                                                                                                                          {
                                                                                                                                                              ResponseCode = "0000"
                                                                                                                                                          }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.BillDetails), TestData.BillDetails}
                                            });
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformBillPaymentMakePostPaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToBillPaymentSuccessPage(), Times.Once);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_SuccessfulPrePayPayment_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentMakePrePaymentRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel()
                                                                                                                                                         {
                                                                                                                                                             ResponseCode = "0000"
                                                                                                                                                         }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.MeterDetails), TestData.MeterDetails}
                                            });
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformBillPaymentMakePrePaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToBillPaymentSuccessPage(), Times.Once);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_FailedPostPayPayment_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentMakePostPaymentRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel() {
                                                                                                                                                                                                               ResponseCode = "1010"
                                                                                                                                                                                                           }));
        
        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.BillDetails), TestData.BillDetails}
                                            });
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformBillPaymentMakePostPaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToBillPaymentFailedPage(), Times.Once);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_FailedPrePayPayment_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentMakePrePaymentRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel()
                                                                                                                                                         {
                                                                                                                                                             ResponseCode = "1010"
                                                                                                                                                         }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.MeterDetails), TestData.MeterDetails}
                                            });
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformBillPaymentMakePrePaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToBillPaymentFailedPage(), Times.Once);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_SetProperties_ValuesExpected(){
        this.ViewModel.PaymentAmount = TestData.PaymentAmount;
        this.ViewModel.CustomerMobileNumber = TestData.CustomerMobileNumber;
        this.ViewModel.IsPostPayVisible = true;
        this.ViewModel.IsPrePayVisible = true;
        this.ViewModel.PaymentAmount.ShouldBe(TestData.PaymentAmount);
        this.ViewModel.CustomerMobileNumber.ShouldBe(TestData.CustomerMobileNumber);
        this.ViewModel.IsPostPayVisible.ShouldBeTrue();
        this.ViewModel.IsPrePayVisible.ShouldBeTrue();

    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.Verify(v => v.GoBack(), Times.Once);
    }
}