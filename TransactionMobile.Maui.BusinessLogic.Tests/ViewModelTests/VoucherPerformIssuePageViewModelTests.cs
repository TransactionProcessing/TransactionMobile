namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests;

using System;
using System.Collections.Generic;
using System.Threading;
using Maui.UIServices;
using MediatR;
using Moq;
using Requests;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherPerformIssuePageViewModelTests
{
    [Fact]
    public void VoucherPerformIssuePageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });

        viewModel.ContractId.ShouldBe(TestData.OperatorId1ContractId.ToString());
        viewModel.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId.ToString());
        viewModel.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
        viewModel.VoucherAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_CustomerEmailAddressEntryCompletedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);
        Boolean isCompletedCalled = false;
        viewModel.OnCustomerEmailAddressEntryCompleted = () =>
                                                         {
                                                             isCompletedCalled = true;
                                                         };

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.CustomerEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_RecipientMobileNumberEntryCompletedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);
        Boolean isCompletedCalled = false;
        viewModel.OnRecipientMobileNumberEntryCompleted = () =>
                                                          {
                                                              isCompletedCalled = true;
                                                          };

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.RecipientMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_RecipientEmailAddressEntryCompleted_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);
        Boolean isCompletedCalled = false;
        viewModel.OnRecipientEmailAddressEntryCompleted = () =>
                                                          {
                                                              isCompletedCalled = true;
                                                          };

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.RecipientEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_VoucherAmountEntryCompletedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);
        Boolean isCompletedCalled = false;
        viewModel.OnVoucherAmountEntryCompleted = () =>
                                                  {
                                                      isCompletedCalled = true;
                                                  };

        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.VoucherAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_IssueVoucherCommand_Execute_SuccessfulVoucher_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);
        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.IssueVoucherCommand.Execute(null);
        mediator.Verify(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(v => v.GoToVoucherIssueSuccessPage(), Times.Once);
    }

    [Fact]
    public void VoucherPerformIssuePageViewModel_IssueVoucherCommand_Execute_FailedVoucher_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        VoucherPerformIssuePageViewModel viewModel = new VoucherPerformIssuePageViewModel(mediator.Object, navigationService.Object);
        viewModel.ApplyQueryAttributes(new Dictionary<String, Object>
                                       {
                                           {nameof(viewModel.ContractId), TestData.OperatorId1ContractId},
                                           {nameof(viewModel.ProductId), TestData.Operator1Product_100KES.ProductId},
                                           {nameof(viewModel.OperatorIdentifier), TestData.OperatorIdentifier1},
                                           {nameof(viewModel.VoucherAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.IssueVoucherCommand.Execute(null);
        mediator.Verify(m => m.Send(It.IsAny<PerformVoucherIssueRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(v => v.GoToVoucherIssueFailedPage(), Times.Once);
    }
}