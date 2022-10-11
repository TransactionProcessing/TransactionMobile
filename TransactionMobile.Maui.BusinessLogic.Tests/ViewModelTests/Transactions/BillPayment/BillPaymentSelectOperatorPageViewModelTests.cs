﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment
{
    using System.Threading;
    using Common;
    using Maui.UIServices;
    using MediatR;
    using Models;
    using Moq;
    using Requests;
    using Services;
    using Shared.Logger;
    using Shouldly;
    using UIServices;
    using ViewModels.Transactions;
    using Xunit;

    public class BillPaymentSelectOperatorPageViewModelTests
    {
        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_Initialise_IsInitialised()
        {
            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
            Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
            BillPaymentSelectOperatorPageViewModel viewModel = new BillPaymentSelectOperatorPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

            await viewModel.Initialise(CancellationToken.None);
            mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

            viewModel.Operators.Count.ShouldBe(3);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
        {
            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
            Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
            Logger.Initialise(NullLogger.Instance);
            BillPaymentSelectOperatorPageViewModel viewModel = new BillPaymentSelectOperatorPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

            await viewModel.Initialise(CancellationToken.None);

            viewModel.Operators.Count.ShouldBe(3);

            ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
            {
                SelectedItemIndex = 1,
                SelectedItem = TestData.ContractOperatorModel
            };

            viewModel.OperatorSelectedCommand.Execute(selectedContractOperator);

            navigationService.Verify(n => n.GoToBillPaymentSelectProductPage(It.IsAny<ProductDetails>()), Times.Once);
        }

        [Fact]
        public async Task BillPaymentSelectOperatorPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            Mock<IMediator> mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
            Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
            Logger.Initialise(NullLogger.Instance);
            BillPaymentSelectOperatorPageViewModel viewModel = new BillPaymentSelectOperatorPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

            ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
            {
                SelectedItemIndex = 1,
                SelectedItem = TestData.ContractOperatorModel
            };

            viewModel.BackButtonCommand.Execute(null);

            navigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }
}