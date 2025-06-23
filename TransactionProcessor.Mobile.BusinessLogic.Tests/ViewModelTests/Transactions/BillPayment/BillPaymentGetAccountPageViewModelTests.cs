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

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class BillPaymentGetAccountPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly BillPaymentGetAccountPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public BillPaymentGetAccountPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new BillPaymentGetAccountPageViewModel(this.NavigationService.Object, this.ApplicationCache.Object, 
                                                                this.DialogSevice.Object, this.DeviceService.Object, this.Mediator.Object,
                                                                this.NavigationParameterService.Object);

        Logger.Initialise(new NullLogger()); 
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        
        this.ViewModel.ProductDetails.ShouldNotBeNull();
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.Operator1ProductDetails.OperatorId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModel));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;
        
        this.ViewModel.GetAccountCommand.Execute(null);
        
        this.NavigationService.Verify(n => n.GoToBillPaymentPayBillPage(It.IsAny<ProductDetails>(), It.IsAny<BillDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Failed_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModelFailed));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        
        this.ViewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;

        this.ViewModel.GetAccountCommand.Execute(null);
        
        this.NavigationService.Verify(n => n.GoToBillPaymentFailedPage(), Times.Once);
    }


    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}