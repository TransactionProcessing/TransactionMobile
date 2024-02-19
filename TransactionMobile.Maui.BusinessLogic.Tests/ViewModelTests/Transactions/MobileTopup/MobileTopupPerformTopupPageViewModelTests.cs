namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using TransactionProcessorACL.DataTransferObjects.Responses;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

[Collection("ViewModelTests")]
public class MobileTopupPerformTopupPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogSevice;
    private readonly MobileTopupPerformTopupPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public MobileTopupPerformTopupPageViewModelTests() {
        
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new MobileTopupPerformTopupPageViewModel(this.Mediator.Object,
                                                                  this.NavigationService.Object,
                                                                  this.ApplicationCache.Object,
                                                                  this.DialogSevice.Object,
                                                                  this.DeviceService.Object);
    }
    [Fact]
    public void MobileTopupPerformTopupPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                            });

        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
        this.ViewModel.TopupAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_CustomerEmailAddressEntryCompletedCommand_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnCustomerEmailAddressEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.CustomerEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_CustomerMobileNumberEntryCompletedCommand_Execute_IsExecuted()
    {
        Boolean isCompletedCalled = false;
        this.ViewModel.OnCustomerMobileNumberEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.CustomerMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_TopupAmountEntryCompletedCommand_Execute_IsExecuted()
    {
        Boolean isCompletedCalled = false;
        this.ViewModel.OnTopupAmountEntryCompleted = () =>
                                                     {
                                                         isCompletedCalled = true;
                                                     };

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.TopupAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_SuccessfulTopup_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<PerformMobileTopupResponseModel>(new PerformMobileTopupResponseModel() {
            ResponseCode = "0000"
        }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.PerformTopupCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToMobileTopupSuccessPage(), Times.Once);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_FailedTopup_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<PerformMobileTopupResponseModel>(new PerformMobileTopupResponseModel()
            {
                ResponseCode = "0001"
            }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.PerformTopupCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToMobileTopupFailedPage(), Times.Once);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.Verify(v => v.GoBack(), Times.Once);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_Properties_ReturnExpectedValues(){
        this.ViewModel.CustomerEmailAddress = TestData.CustomerEmailAddress;
        this.ViewModel.CustomerMobileNumber= TestData.CustomerMobileNumber;

        this.ViewModel.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
        this.ViewModel.CustomerMobileNumber.ShouldBe(TestData.CustomerMobileNumber);
    }
}