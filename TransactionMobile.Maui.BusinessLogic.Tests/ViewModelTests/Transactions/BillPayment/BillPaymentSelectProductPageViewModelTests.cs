namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

public class BillPaymentSelectProductPageViewModelTests
{
    #region Methods

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        BillPaymentSelectProductPageViewModel viewModel = new BillPaymentSelectProductPageViewModel(mediator.Object, navigationService.Object,
                                                                                                    applicationCache.Object,
                                                                                                    dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        viewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
    }

    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_Initialise_IsInitialised()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        BillPaymentSelectProductPageViewModel viewModel = new BillPaymentSelectProductPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Products.Count.ShouldBe(3);
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_ProductSelectedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentSelectProductPageViewModel viewModel = new BillPaymentSelectProductPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        await viewModel.Initialise(CancellationToken.None);
        mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        viewModel.Products.Count.ShouldBe(3);

        ItemSelected<ContractProductModel> selectedContractProduct = new ItemSelected<ContractProductModel>
                                                                     {
                                                                         SelectedItemIndex = 1,
                                                                         SelectedItem = TestData.Operator1Product_100KES
                                                                     };

        viewModel.ProductSelectedCommand.Execute(selectedContractProduct);

        navigationService.Verify(n => n.GoToBillPaymentGetAccountPage(It.IsAny<ProductDetails>()), Times.Once);
    }

    [Fact]
    public async Task MobileTopupSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentSelectProductPageViewModel viewModel = new BillPaymentSelectProductPageViewModel(mediator.Object, navigationService.Object, applicationCache.Object, dialogSevice.Object);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoBack(), Times.Once);
    }

    #endregion
}

public class BillPaymentGetAccountPageViewModelTests
{
    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.ContractProductList);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        BillPaymentGetAccountPageViewModel viewModel = new BillPaymentGetAccountPageViewModel(navigationService.Object,
                                                                                              applicationCache.Object,
                                                                                              dialogSevice.Object,
                                                                                              mediator.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        viewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.Operator1ProductDetails.OperatorIdentifier);
        viewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        viewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformBillPaymentGetAccountResponseModel);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentGetAccountPageViewModel viewModel = new BillPaymentGetAccountPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object, mediator.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        viewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;
        
        viewModel.GetAccountCommand.Execute(null);
        
        navigationService.Verify(n => n.GoToBillPaymentPayBillPage(It.IsAny<ProductDetails>(), It.IsAny<BillDetails>()), Times.Once);
    }

    [Fact]
    public async Task BillPaymentGetAccountPageViewModel_GetAccountCommand_Failed_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformBillPaymentGetAccountResponseModelFailed);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentGetAccountPageViewModel viewModel = new BillPaymentGetAccountPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object, mediator.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                      });
        viewModel.CustomerAccountNumber = TestData.CustomerAccountNumber;

        viewModel.GetAccountCommand.Execute(null);
        
        navigationService.Verify(n => n.GoToBillPaymentFailedPage(), Times.Once);
    }


    [Fact]
    public async Task BillPaymentSelectProductPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentGetAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PerformBillPaymentGetAccountResponseModelFailed);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentGetAccountPageViewModel viewModel = new BillPaymentGetAccountPageViewModel(navigationService.Object, applicationCache.Object, dialogSevice.Object, mediator.Object);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoBack(), Times.Once);
    }
}

public class BillPaymentPayBillPageViewModelTests
{
    [Fact]
    public async Task BillPaymentPayBillPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        BillPaymentPayBillPageViewModel viewModel = new BillPaymentPayBillPageViewModel(navigationService.Object,
                                                                                        applicationCache.Object,
                                                                                        dialogSevice.Object,
                                                                                        mediator.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object> {
                                                                          {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                                          {nameof(BillDetails), TestData.BillDetails}
                                                                      });

        viewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.Operator1ProductDetails.OperatorIdentifier);
        viewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1ProductDetails.ProductId);
        viewModel.ProductDetails.ContractId.ShouldBe(TestData.Operator1ProductDetails.ContractId);
        viewModel.BillDetails.AccountName.ShouldBe(TestData.BillDetails.AccountName);
        viewModel.BillDetails.AccountNumber.ShouldBe(TestData.BillDetails.AccountNumber);
        viewModel.BillDetails.Balance.ShouldBe(TestData.BillDetails.Balance);
        viewModel.BillDetails.DueDate.ShouldBe(TestData.BillDetails.DueDate);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_CustomerMobileNumberEntryCompleted_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentPayBillPageViewModel viewModel = new BillPaymentPayBillPageViewModel(navigationService.Object,
                                                                                        applicationCache.Object,
                                                                                        dialogSevice.Object,
                                                                                        mediator.Object);
        bool isCompletedCalled = false;
        viewModel.OnCustomerMobileNumberEntryCompleted = () =>
        {
            isCompletedCalled = true;
        };

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(BillDetails), TestData.BillDetails}
                                       });
        viewModel.CustomerMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_PaymentAmountEntryCompleted_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentPayBillPageViewModel viewModel = new BillPaymentPayBillPageViewModel(navigationService.Object,
                                                                                        applicationCache.Object,
                                                                                        dialogSevice.Object,
                                                                                        mediator.Object);
        bool isCompletedCalled = false;
        viewModel.OnPaymentAmountEntryCompleted = () =>
                                                         {
                                                             isCompletedCalled = true;
                                                         };

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(BillDetails), TestData.BillDetails}
                                       });
        viewModel.PaymentAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_SuccessfulPayment_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentMakePaymentRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentPayBillPageViewModel viewModel = new BillPaymentPayBillPageViewModel(navigationService.Object,
                                                                                        applicationCache.Object,
                                                                                        dialogSevice.Object,
                                                                                        mediator.Object);
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.BillDetails), TestData.BillDetails}
                                       });
        viewModel.MakeBillPaymentCommand.Execute(null);
        mediator.Verify(m => m.Send(It.IsAny<PerformBillPaymentMakePaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(v => v.GoToBillPaymentSuccessPage(), Times.Once);
    }

    [Fact]
    public void BillPaymentPayBillPageViewModel_MakeBillPaymentCommand_Execute_FailedPayment_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformBillPaymentMakePaymentRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        BillPaymentPayBillPageViewModel viewModel = new BillPaymentPayBillPageViewModel(navigationService.Object,
                                                                                        applicationCache.Object,
                                                                                        dialogSevice.Object,
                                                                                        mediator.Object);
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.BillDetails), TestData.BillDetails}
                                       });
        viewModel.MakeBillPaymentCommand.Execute(null);
        mediator.Verify(m => m.Send(It.IsAny<PerformBillPaymentMakePaymentRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(v => v.GoToBillPaymentFailedPage(), Times.Once);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupPerformTopupPageViewModel viewModel = new MobileTopupPerformTopupPageViewModel(mediator.Object, navigationService.Object,
                                                                                                  applicationCache.Object,
                                                                                                  dialogSevice.Object);

        viewModel.BackButtonCommand.Execute(null);
        navigationService.Verify(v => v.GoBack(), Times.Once);
    }
}