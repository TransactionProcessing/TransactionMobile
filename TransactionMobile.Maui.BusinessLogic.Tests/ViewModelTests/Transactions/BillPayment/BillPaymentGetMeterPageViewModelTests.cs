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
using Requests;
using Services;
using Shouldly;
using SimpleResults;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class BillPaymentGetMeterPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentGetMeterPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public BillPaymentGetMeterPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new BillPaymentGetMeterPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object,
                                                              this.DialogSevice.Object, this.DeviceService.Object, this.Mediator.Object);

        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.Operator1ProductDetails.OperatorIdentifier);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetMeterCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetMeterRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModel));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        this.ViewModel.MeterNumber = TestData.MeterNumber;

        this.ViewModel.GetMeterCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoToBillPaymentPayBillPage(It.IsAny<ProductDetails>(), It.IsAny<MeterDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetMeterCommand_Failed_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetMeterRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModelFailed));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                               {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                           });
        this.ViewModel.MeterNumber = TestData.MeterNumber;

        this.ViewModel.GetMeterCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoToBillPaymentFailedPage(), Times.Once);
    }


    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}