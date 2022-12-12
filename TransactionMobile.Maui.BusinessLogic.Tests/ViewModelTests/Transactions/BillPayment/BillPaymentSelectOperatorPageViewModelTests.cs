using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment
{
    using System.Collections.Generic;
    using System.Threading;
    using Common;
    using Maui.UIServices;
    using MediatR;
    using Models;
    using Moq;
    using RequestHandlers;
    using Requests;
    using Services;
    using Shared.Logger;
    using Shouldly;
    using UIServices;
    using ViewModels.Transactions;
    using Xunit;

    public class BillPaymentSelectOperatorPageViewModelTests
    {
        private readonly Mock<IMediator> Mediator;

        private readonly Mock<INavigationService> NavigationService;

        private readonly Mock<IApplicationCache> ApplicationCache;

        private readonly Mock<IDialogService> DialogSevice;

        private readonly BillPaymentSelectOperatorPageViewModel ViewModel;

        public BillPaymentSelectOperatorPageViewModelTests() {
            this.Mediator = new Mock<IMediator>();
            this.NavigationService = new Mock<INavigationService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogSevice = new Mock<IDialogService>();
            this.ViewModel = new BillPaymentSelectOperatorPageViewModel(this.Mediator.Object, this.NavigationService.Object, this.ApplicationCache.Object, this.DialogSevice.Object);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_Initialise_IsInitialised()
        {
            this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));

            await this.ViewModel.Initialise(CancellationToken.None);
            this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            this.ViewModel.Operators.Count.ShouldBe(3);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
        {
            this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<List<ContractProductModel>>(TestData.ContractProductList));

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
