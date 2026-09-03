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

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

[Collection("ViewModelTests")]
public class VoucherPerformIssuePageViewModelTests
{
    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;

    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogService;
    private readonly VoucherPerformIssuePageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public VoucherPerformIssuePageViewModelTests() {
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new VoucherPerformIssuePageViewModel(this.NavigationService.Instance(), this.ApplicationCache.Instance(),
                                                              this.DialogService.Instance(), this.DeviceService.Instance(), this.Mediator.Instance(), this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task VoucherPerformIssuePageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.OperatorId1);
        this.ViewModel.VoucherAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_CustomerEmailAddressEntryCompletedCommand_Execute_IsExecuted()
    {                                                                    
        bool isCompletedCalled = false;
        this.ViewModel.OnCustomerEmailAddressEntryCompleted = () =>
                                                              {
                                                                  isCompletedCalled = true;
                                                              };
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        });
        this.ViewModel.CustomerEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_RecipientMobileNumberEntryCompletedCommand_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnRecipientMobileNumberEntryCompleted = () =>
                                                               {
                                                                   isCompletedCalled = true;
                                                               };
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        });
        this.ViewModel.RecipientMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_RecipientEmailAddressEntryCompleted_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnRecipientEmailAddressEntryCompleted = () =>
                                                               {
                                                                   isCompletedCalled = true;
                                                               };

        // TODO: Move to the mock
        //this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
        //                                    {
        //                                        {nameof(ProductDetails), TestData.Operator1ProductDetails},
        //                                        {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        //                                    });
        this.ViewModel.RecipientEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task VoucherPerformIssuePageViewModel_VoucherAmountEntryCompletedCommand_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnVoucherAmountEntryCompleted = () =>
                                                       {
                                                           isCompletedCalled = true;
                                                       };

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails},
            {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.VoucherAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public async Task VoucherPerformIssuePageViewModel_IssueVoucherCommand_Execute_SuccessfulVoucher_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformVoucherIssueResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformVoucherIssueResponseModel() {
                                                                                                                                                                                       ResponseCode = "0000"
                                                                                                                                                                                   }));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.IssueVoucherCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformVoucherIssueResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToVoucherIssueSuccessPage().Called(Count.Once());
    }

    [Fact]
    public async Task VoucherPerformIssuePageViewModel_IssueVoucherCommand_Execute_FailedVoucher_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<PerformVoucherIssueResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new PerformVoucherIssueResponseModel()
                                                                                                                                            {
                                                                                                                                                ResponseCode = "1010"
                                                                                                                                            }));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.IssueVoucherCommand.Execute(null);
        this.Mediator.Send(Arg<IRequest<Result<PerformVoucherIssueResponseModel>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.NavigationService.GoToVoucherIssueFailedPage().Called(Count.Once());
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_Properties_ReturnExpectedValues()
    {
        this.ViewModel.CustomerEmailAddress = TestData.CustomerEmailAddress;
        this.ViewModel.RecipientEmailAddress = TestData.RecipientEmailAddress;
        this.ViewModel.RecipientMobileNumber= TestData.RecipientMobileNumber;

        this.ViewModel.CustomerEmailAddress.ShouldBe(TestData.CustomerEmailAddress);
        this.ViewModel.RecipientEmailAddress.ShouldBe(TestData.RecipientEmailAddress);
        this.ViewModel.RecipientMobileNumber.ShouldBe(TestData.RecipientMobileNumber);
    }
}
