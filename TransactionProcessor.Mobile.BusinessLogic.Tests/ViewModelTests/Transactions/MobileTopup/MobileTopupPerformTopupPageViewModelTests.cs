using MediatR;
using Moq;
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
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogSevice;
    private readonly MobileTopupPerformTopupPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public MobileTopupPerformTopupPageViewModelTests() {
        
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new MobileTopupPerformTopupPageViewModel(this.Mediator.Object,
                                                                  this.NavigationService.Object,
                                                                  this.ApplicationCache.Object,
                                                                  this.DialogSevice.Object,
                                                                  this.DeviceService.Object,
                                                                  this.NavigationParameterService.Object);
    }
    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
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

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
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

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
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

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
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
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel() {
                                                                                                                                                                                     ResponseCode = "0000"
                                                                                                                                                                                 }));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.PerformTopupCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToMobileTopupSuccessPage(), Times.Once);
    }

    [Fact]
    public async Task MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_FailedTopup_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new PerformMobileTopupResponseModel()
                                                                                                                                           {
                                                                                                                                               ResponseCode = "0001"
                                                                                                                                           }));

        this.NavigationParameterService.Setup(n => n.GetParameters()).Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
            {nameof(this.ViewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
        });
        await this.ViewModel.Initialise(CancellationToken.None);

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