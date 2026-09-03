using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using ProductDetails = TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions.ProductDetails;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

[Collection("ViewModelTests")]
public class BillPaymentSelectProductPageViewModelTests
{
    #region Methods

    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;

    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogSevice;

    private readonly BillPaymentSelectProductPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public BillPaymentSelectProductPageViewModelTests()
    {
        this.Mediator = new IMediatorImposter();
        
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new BillPaymentSelectProductPageViewModel(this.Mediator.Instance(),
                                                                   this.NavigationService.Instance(),
                                                                   this.ApplicationCache.Instance(),
                                                                   this.DialogSevice.Instance(),
                                                                   this.DeviceService.Instance(),
                                                                   this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.OperatorId1);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

        this.ViewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ProductSelectedCommand_PostPay_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));
        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

        this.ViewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_BillPayment_PostPayment,
                                                                     };

        this.ViewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        this.NavigationService.GoToBillPaymentGetAccountPage(Arg<ProductDetails>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ProductSelectedCommand_PrePay_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> { { nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel }, });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

        this.ViewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_BillPayment_PrePayment,
                                                                     };

        this.ViewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        this.NavigationService.GoToBillPaymentGetMeterPage(Arg<ProductDetails>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }

    #endregion
}