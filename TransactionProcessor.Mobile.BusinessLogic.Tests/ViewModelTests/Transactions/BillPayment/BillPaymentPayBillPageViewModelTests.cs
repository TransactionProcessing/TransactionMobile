using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using BillDetails = TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions.BillDetails;
using MeterDetails = TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions.MeterDetails;
using ProductDetails = TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions.ProductDetails;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

[Collection("ViewModelTests")]
public class BillPaymentPayBillPageViewModelTests
{
    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;

    private INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogSevice;

    private readonly BillPaymentPayBillPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public BillPaymentPayBillPageViewModelTests() {
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new BillPaymentPayBillPageViewModel(this.NavigationService.Instance(), this.ApplicationCache.Instance(),
                               this.DialogSevice.Instance(), this.Mediator.Instance(), this.DeviceService.Instance(),
                               this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_ApplyQueryAttributes_PostPay_QueryAttributesApplied()
    {
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(BillDetails), TestData.BillDetails_ViewModel}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.OperatorId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.ContractId);
        this.ViewModel.BillDetails.AccountName.ShouldBe(TestData.BillDetails_ViewModel.AccountName);
        this.ViewModel.BillDetails.AccountNumber.ShouldBe(TestData.BillDetails_ViewModel.AccountNumber);
        this.ViewModel.BillDetails.Balance.ShouldBe(TestData.BillDetails_ViewModel.Balance);
        this.ViewModel.BillDetails.DueDate.ShouldBe(TestData.BillDetails_ViewModel.DueDate);
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_ApplyQueryAttributes_PrePay_QueryAttributesApplied()
    {
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(MeterDetails), TestData.MeterDetails_ViewModel}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.OperatorId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.ProductId);
        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails_ViewModel.ContractId);
        this.ViewModel.MeterDetails.MeterNumber.ShouldBe(TestData.MeterDetails_ViewModel.MeterNumber);
        this.ViewModel.MeterDetails.CustomerName.ShouldBe(TestData.MeterDetails_ViewModel.CustomerName);
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_CustomerMobileNumberEntryCompleted_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnCustomerMobileNumberEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(BillDetails), TestData.BillDetails}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.CustomerMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_PaymentAmountEntryCompleted_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnPaymentAmountEntryCompleted = () =>
                                                       {
                                                           isCompletedCalled = true;
                                                       };
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(BillDetails), TestData.BillDetails}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.PaymentAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_SuccessfulPostPayPayment_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel()
                                                                                                                                                          {
                                                                                                                                                              ResponseCode = "0000"
                                                                                                                                                          }));
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(BillDetails), TestData.BillDetails}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToBillPaymentSuccessPage().Called(Count.Once());
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_SuccessfulPrePayPayment_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel()
                                                                                                                                                         {
                                                                                                                                                             ResponseCode = "0000"
                                                                                                                                                         }));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(MeterDetails), TestData.MeterDetails}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToBillPaymentSuccessPage().Called(Count.Once());
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_FailedPostPayPayment_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel() {
                                                                                                                                                                                                               ResponseCode = "1010"
                                                                                                                                                                                                           }));
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(BillDetails), TestData.BillDetails}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToBillPaymentFailedPage().Called(Count.Once());
    }

    [Fact]
    public async Task BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_FailedPrePayPayment_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformBillPaymentMakePaymentResponseModel()
                                                                                                                                                         {
                                                                                                                                                             ResponseCode = "1010"
                                                                                                                                                         }));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(MeterDetails), TestData.MeterDetails}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.MakeBillPaymentCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformBillPaymentMakePaymentResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToBillPaymentFailedPage().Called(Count.Once());
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
        this.NavigationService.GoBack().Called(Count.Once());
    }
}
