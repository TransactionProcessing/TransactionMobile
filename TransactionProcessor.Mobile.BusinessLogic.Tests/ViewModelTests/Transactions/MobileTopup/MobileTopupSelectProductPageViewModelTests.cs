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

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

[Collection("ViewModelTests")]
public class MobileTopupSelectProductPageViewModelTests
{
    #region Methods

    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;
    private INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogSevice;

    private readonly MobileTopupSelectProductPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public MobileTopupSelectProductPageViewModelTests() {
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new MobileTopupSelectProductPageViewModel(this.Mediator.Instance(), this.NavigationService.Instance(), this.ApplicationCache.Instance(), this.DialogSevice.Instance(), this.DeviceService.Instance(),
            this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied() {
        
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
        });
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.ProductDetails.OperatorId.ShouldBe(TestData.OperatorId1);
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_Initialise_IsInitialised() {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

        this.ViewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_ProductSelectedCommand_Execute_IsExecuted() {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<String, Object> {
            {nameof(ProductDetails), TestData.Operator1ProductDetails_ViewModel},
        });
        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

        this.ViewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel> {
                                                                                                                SelectedItemIndex = 1,
                                                                                                                SelectedItem = TestData.Operator1Product_100KES
                                                                                                            };

        this.ViewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        this.NavigationService.GoToMobileTopupPerformTopupPage(Arg<ProductDetails>.Any(),Arg<Decimal>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));
        
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }

    #endregion
}