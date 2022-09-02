namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.MyAccount;

using System.Threading;
using System.Threading.Tasks;
using Maui.UIServices;
using MediatR;
using Moq;
using Services;
using Shared.Logger;
using Shouldly;
using UIServices;
using ViewModels.MyAccount;
using Xunit;

public class MyAccountDetailsPageViewModelTests
{
    [Fact]
    public async Task MyAccountDetailsPageViewModel_Initialise_IsInitialised()
    {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(TestData.MerchantDetailsModel);
        Mock<IDialogService> dialogService = new Mock<IDialogService>();

        MyAccountDetailsPageViewModel viewModel = new MyAccountDetailsPageViewModel(navigationService.Object,
                                                                                    applicationCache.Object,
                                                                                    dialogService.Object);
        await viewModel.Initialise(CancellationToken.None);

        applicationCache.Verify(a => a.GetMerchantDetails(), Times.Once);
        viewModel.Balance.ShouldBe(TestData.Balance);
        viewModel.AvailableBalance.ShouldBe(TestData.AvailableBalance);
        viewModel.MerchantName.ShouldBe(TestData.MerchantName);
        viewModel.LastStatementDate.ShouldBe(TestData.LastStatementDate);
        viewModel.NextStatementDate.ShouldBe(TestData.NextStatementDate);
        viewModel.SettlementSchedule.ShouldBe(TestData.SettlementSchedule);
    }

    [Fact]
    public async Task MyAccountDetailsPageViewModel_BackButtonCommand_PreviousPageIsShown()
    {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogService = new Mock<IDialogService>();
        MyAccountDetailsPageViewModel viewModel = new MyAccountDetailsPageViewModel(navigationService.Object, applicationCache.Object,
                                                                                    dialogService.Object);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoBack(), Times.Once);
    }
}