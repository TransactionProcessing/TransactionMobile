using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using ProductDetails = TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions.ProductDetails;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

[Collection("ViewModelTests")]
public class MobileTopupPerformTopupPageViewModelTests
{
    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly IDialogServiceImposter DialogSevice;
    private readonly MobileTopupPerformTopupPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public MobileTopupPerformTopupPageViewModelTests() {
        
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new MobileTopupPerformTopupPageViewModel(this.Mediator.Instance(),
                                                                  this.NavigationService.Instance(),
                                                                  this.ApplicationCache.Instance(),
                                                                  this.DialogSevice.Instance(),
                                                                  this.DeviceService.Instance(),
                                                                  this.NavigationParameterService.Instance());
    }
    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.OperatorId1);
        this.ViewModel.TopupAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
    }

    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_CustomerEmailAddressEntryCompletedCommand_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnCustomerEmailAddressEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.CustomerEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_CustomerMobileNumberEntryCompletedCommand_Execute_IsExecuted()
    {
        Boolean isCompletedCalled = false;
        this.ViewModel.OnCustomerMobileNumberEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.CustomerMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_TopupAmountEntryCompletedCommand_Execute_IsExecuted()
    {
        Boolean isCompletedCalled = false;
        this.ViewModel.OnTopupAmountEntryCompleted = () =>
                                                     {
                                                         isCompletedCalled = true;
                                                     };

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.TopupAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_SuccessfulTopup_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformMobileTopupResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel() {
                                                                                                                                                                                     ResponseCode = "0000"
                                                                                                                                                                                 }));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.PerformTopupCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformMobileTopupResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToMobileTopupSuccessPage().Called(Count.Once());
    }

    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_FailedTopup_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformMobileTopupResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel()
                                                                                                                                           {
                                                                                                                                               ResponseCode = "0001"
                                                                                                                                           }));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.PerformTopupCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformMobileTopupResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToMobileTopupFailedPage().Called(Count.Once());
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);
        this.NavigationService.GoBack().Called(Count.Once());
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_Properties_ReturnExpectedValues(){
        this.ViewModel.CustomerEmailAddress = TestData.CustomerEmailAddress;
        this.ViewModel.CustomerMobileNumber= TestData.CustomerMobileNumber;

        this.ViewModel.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
        this.ViewModel.CustomerMobileNumber.ShouldBe(TestData.CustomerMobileNumber);
    }
}