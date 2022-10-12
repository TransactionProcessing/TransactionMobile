namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using System;
using System.Collections.Generic;
using System.Threading;
using Maui.UIServices;
using MediatR;
using Moq;
using Requests;
using Services;
using Shared.Logger;
using Shouldly;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupPerformTopupPageViewModelTests
{
    [Fact]
    public void MobileTopupPerformTopupPageViewModel_ApplyQueryAttributes_QueryAttributesApplied()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupPerformTopupPageViewModel viewModel = new MobileTopupPerformTopupPageViewModel(mediator.Object,
                                                                                                  navigationService.Object,
                                                                                                  applicationCache.Object,
                                                                                                  dialogSevice.Object);

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                       });

        viewModel.ProductDetails.ContractId.ShouldBe(TestData.OperatorId1ContractId);
        viewModel.ProductDetails.ProductId.ShouldBe(TestData.Operator1Product_100KES.ProductId);
        viewModel.ProductDetails.OperatorIdentifier.ShouldBe(TestData.OperatorIdentifier1);
        viewModel.TopupAmount.ShouldBe(TestData.Operator1Product_100KES.Value);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_CustomerEmailAddressEntryCompletedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupPerformTopupPageViewModel viewModel = new MobileTopupPerformTopupPageViewModel(mediator.Object, navigationService.Object,
                                                                                                  applicationCache.Object,dialogSevice.Object);
        bool isCompletedCalled = false;
        viewModel.OnCustomerEmailAddressEntryCompleted = () =>
                                                         {
                                                             isCompletedCalled = true;
                                                         };

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.CustomerEmailAddressEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_CustomerMobileNumberEntryCompletedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupPerformTopupPageViewModel viewModel = new MobileTopupPerformTopupPageViewModel(mediator.Object, navigationService.Object,applicationCache.Object,
                                                                                                  dialogSevice.Object);
        Boolean isCompletedCalled = false;
        viewModel.OnCustomerMobileNumberEntryCompleted = () =>
                                                         {
                                                             isCompletedCalled = true;
                                                         };

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.CustomerMobileNumberEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_TopupAmountEntryCompletedCommand_Execute_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupPerformTopupPageViewModel viewModel = new MobileTopupPerformTopupPageViewModel(mediator.Object, navigationService.Object,
                                                                                                  applicationCache.Object,
                                                                                                  dialogSevice.Object);
        Boolean isCompletedCalled = false;
        viewModel.OnTopupAmountEntryCompleted = () =>
                                                {
                                                    isCompletedCalled = true;
                                                };

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.TopupAmountEntryCompletedCommand.Execute(null);
        isCompletedCalled.ShouldBeTrue();
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_SuccessfulTopup_IsExecuted()
    {
        Mock<IMediator> mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogSevice = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        MobileTopupPerformTopupPageViewModel viewModel = new MobileTopupPerformTopupPageViewModel(mediator.Object, navigationService.Object,
                                                                                                  applicationCache.Object,
                                                                                                  dialogSevice.Object);
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.PerformTopupCommand.Execute(null);
        mediator.Verify(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(v => v.GoToMobileTopupSuccessPage(), Times.Once);
    }

    [Fact]
    public void MobileTopupPerformTopupPageViewModel_PerformTopupCommand_Execute_FailedTopup_IsExecuted()
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
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
                                       {
                                           {nameof(ProductDetails), TestData.Operator1ProductDetails},
                                           {nameof(viewModel.TopupAmount), TestData.Operator1Product_100KES.Value}
                                       });
        viewModel.PerformTopupCommand.Execute(null);
        mediator.Verify(m => m.Send(It.IsAny<PerformMobileTopupRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        navigationService.Verify(v => v.GoToMobileTopupFailedPage(), Times.Once);
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