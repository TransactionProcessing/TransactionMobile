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
public class MobileTopupSelectOperatorPageViewModelTests
{
    private readonly IMediatorImposter Mediator;

    private readonly INavigationServiceImposter NavigationService;

    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogSevice;

    private readonly MobileTopupSelectOperatorPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public MobileTopupSelectOperatorPageViewModelTests() {
        this.Mediator = new IMediatorImposter();
        
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogSevice = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new MobileTopupSelectOperatorPageViewModel(this.Mediator.Instance(), this.NavigationService.Instance(), this.DialogSevice.Instance(), this.ApplicationCache.Instance(), this.DeviceService.Instance(),
            this.NavigationParameterService.Instance());

        
    }

    [Fact]
    public async Task MobileTopupSelectOperatorPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractOperatorModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractOperatorList));

        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Send(Arg<IRequest<Result<List<ContractOperatorModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

        this.ViewModel.Operators.Count.ShouldBe(1);
    }

    [Fact]
    public async Task MobileTopupSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractOperatorModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractOperatorList));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.Operators.Count.ShouldBe(1);

        ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
                                                                       {
                                                                           SelectedItemIndex = 1,
                                                                           SelectedItem = TestData.ContractOperatorModel
                                                                       };

        this.ViewModel.OperatorSelectedCommand.Execute(selectedContractOperator);
        
        this.NavigationService.GoToMobileTopupSelectProductPage(Arg<ProductDetails>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task MobileTopupSelectOperatorPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.Mediator.Send(Arg<IRequest<Result<List<ContractProductModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractProductList));
        
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }
}