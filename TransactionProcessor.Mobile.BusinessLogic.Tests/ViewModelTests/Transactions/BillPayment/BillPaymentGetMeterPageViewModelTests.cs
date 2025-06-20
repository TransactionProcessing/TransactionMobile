using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

public class BillPaymentGetMeterPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentGetMeterPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public BillPaymentGetMeterPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new BillPaymentGetMeterPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object,
                                                              this.DialogSevice.Object, this.DeviceService.Object, this.Mediator.Object,
                                                              this.NavigationParameterService.Object);

        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.OperatorId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.ContractId);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetMeterCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetMeterRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModel));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.MeterNumber = TestData.MeterNumber;

        this.ViewModel.GetMeterCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoToBillPaymentPayBillPage(It.IsAny<ProductDetails>(), It.IsAny<Models.MeterDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetMeterCommand_Failed_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetMeterRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetMeterResponseModelFailed));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
        });
        await this.ViewModel.Initialise(CancellationToken.None);
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