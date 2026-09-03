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

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment
{
    [Collection("ViewModelTests")]
    public class BillPaymentSelectOperatorPageViewModelTests
    {
        private readonly IMediatorImposter Mediator;

        private readonly INavigationServiceImposter NavigationService;

        private readonly INavigationParameterServiceImposter NavigationParameterService;

        private readonly IApplicationCacheImposter ApplicationCache;

        private readonly IDialogServiceImposter DialogSevice;

        private readonly BillPaymentSelectOperatorPageViewModel ViewModel;

        private readonly IDeviceServiceImposter DeviceService;

        public BillPaymentSelectOperatorPageViewModelTests() {
            this.Mediator = new IMediatorImposter();
            this.NavigationParameterService = new INavigationParameterServiceImposter();
            this.NavigationService = new INavigationServiceImposter();
            this.ApplicationCache = new IApplicationCacheImposter();
            this.DialogSevice = new IDialogServiceImposter();
            this.DeviceService = new IDeviceServiceImposter();
            this.ViewModel = new BillPaymentSelectOperatorPageViewModel(this.Mediator.Instance(),
                                                                        this.NavigationService.Instance(),
                                                                        this.ApplicationCache.Instance(),
                                                                        this.DialogSevice.Instance(),
                                                                        this.DeviceService.Instance(),
                                                                        this.NavigationParameterService.Instance());
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_Initialise_IsInitialised()
        {
            this.Mediator.Send(Arg<IRequest<Result<List<ContractOperatorModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractOperatorList));

            await this.ViewModel.Initialise(CancellationToken.None);
            this.Mediator.Send(Arg<IRequest<Result<List<ContractOperatorModel>>>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());

            this.ViewModel.Operators.Count.ShouldBe(1);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
        {
            this.Mediator.Send(Arg<IRequest<Result<List<ContractOperatorModel>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.ContractOperatorList));

            await this.ViewModel.Initialise(CancellationToken.None);

            this.ViewModel.Operators.Count.ShouldBe(1);

            ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
                                                                           {
                                                                               SelectedItemIndex = 0,
                                                                               SelectedItem = TestData.ContractOperatorModel
                                                                           };

            this.ViewModel.OperatorSelectedCommand.Execute(selectedContractOperator);

            this.NavigationService.GoToBillPaymentSelectProductPage(Arg<ProductDetails>.Any()).Called(Count.Once());
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
            {
                SelectedItemIndex = 1,
                SelectedItem = TestData.ContractOperatorModel
            };

            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.GoBack().Called(Count.Once());
        }
    }
}
