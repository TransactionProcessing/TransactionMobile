using MediatR;
using Moq;
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
        private readonly Mock<IMediator> Mediator;

        private readonly Mock<INavigationService> NavigationService;

        private readonly Mock<INavigationParameterService> NavigationParameterService;

        private readonly Mock<IApplicationCache> ApplicationCache;

        private readonly Mock<IDialogService> DialogSevice;

        private readonly BillPaymentSelectOperatorPageViewModel ViewModel;

        private readonly Mock<IDeviceService> DeviceService;

        public BillPaymentSelectOperatorPageViewModelTests() {
            this.Mediator = new Mock<IMediator>();
            this.NavigationParameterService = new Mock<INavigationParameterService>();
            this.NavigationService = new Mock<INavigationService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogSevice = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            this.ViewModel = new BillPaymentSelectOperatorPageViewModel(this.Mediator.Object,
                                                                        this.NavigationService.Object,
                                                                        this.ApplicationCache.Object,
                                                                        this.DialogSevice.Object,
                                                                        this.DeviceService.Object,
                                                                        this.NavigationParameterService.Object);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_Initialise_IsInitialised()
        {
            this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

            await this.ViewModel.Initialise(CancellationToken.None);
            this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            this.ViewModel.Operators.Count.ShouldBe(3);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
        {
            this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));

            await this.ViewModel.Initialise(CancellationToken.None);

            this.ViewModel.Operators.Count.ShouldBe(3);

            ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
                                                                           {
                                                                               SelectedItemIndex = 1,
                                                                               SelectedItem = TestData.ContractOperatorModel
                                                                           };

            this.ViewModel.OperatorSelectedCommand.Execute(selectedContractOperator);

            this.NavigationService.Verify(n => n.GoToBillPaymentSelectProductPage(It.IsAny<ProductDetails>()), Times.Once);
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

            this.NavigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }
}
