using MediatR;
using Imposter.Abstractions;
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
    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;

    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogSevice;

    private readonly BillPaymentGetAccountPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public BillPaymentGetAccountPageViewModelTests() {
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new BillPaymentGetAccountPageViewModel(this.NavigationService.Instance(), this.ApplicationCache.Instance(),
                                                                this.DialogSevice.Instance(), this.DeviceService.Instance(), this.Mediator.Instance(),
                                                                this.NavigationParameterService.Instance());

        Logger.Initialise(new NullLogger()); 
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        
        this.ViewModel.ProductDetails.ShouldNotBeNull();
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.Operator1ProductDetails.OperatorId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentGetAccountResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModel));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;
        
        this.ViewModel.GetAccountCommand.Execute(null);
        
        this.NavigationService.GoToBillPaymentPayBillPage(Arg<ProductDetails>.Any(), Arg<BillDetails>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Failed_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentGetAccountResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.PerformBillPaymentGetAccountResponseModelFailed));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        
        this.ViewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;

        this.ViewModel.GetAccountCommand.Execute(null);
        
        this.NavigationService.GoToBillPaymentFailedPage().Called(Count.Once());
    }


    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }
}
