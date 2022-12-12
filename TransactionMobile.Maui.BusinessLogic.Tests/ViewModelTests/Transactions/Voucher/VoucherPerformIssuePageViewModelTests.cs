namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using System;
using System.Collections.Generic;
using System.Threading;
using Maui.UIServices;
using MediatR;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shared.Logger;
using Shouldly;
using TransactionProcessorACL.DataTransferObjects.Responses;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherPerformIssuePageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly VoucherPerformIssuePageViewModel ViewModel;

    public VoucherPerformIssuePageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.ViewModel = new VoucherPerformIssuePageViewModel(this.NavigationService.Object, this.ApplicationCache.Object, this.DialogService.Object, this.Mediator.Object);
        Logger.Initialise(NullLogger.Instance);
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                            });

        this.ViewModel.ProductDetails.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        this.ViewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        this.ViewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
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

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
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

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
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

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.RecipientEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_VoucherAmountEntryCompletedCommand_Execute_IsExecuted()
    {
        bool isCompletedCalled = false;
        this.ViewModel.OnVoucherAmountEntryCompleted = () =>
                                                       {
                                                           isCompletedCalled = true;
                                                       };

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.VoucherAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_IssueVoucherCommand_Execute_SuccessfulVoucher_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<SaleTransactionResponseMessage>(new SaleTransactionResponseMessage() {
            ResponseCode = "0000"
        }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.IssueVoucherCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToVoucherIssueSuccessPage(), Times.Once);
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_IssueVoucherCommand_Execute_FailedVoucher_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<SaleTransactionResponseMessage>(new SaleTransactionResponseMessage()
            {
                ResponseCode = "1010"
            }));

        this.ViewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                            {
                                                {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                                {nameof(this.ViewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                            });
        this.ViewModel.IssueVoucherCommand.Execute(null);
        this.Mediator.Verify(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        this.NavigationService.Verify(v => v.GoToVoucherIssueFailedPage(), Times.Once);
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(v => v.GoBack(), Times.Once);
    }
}