using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.MyAccount;

[Collection("ViewModelTests")]
public class MyAccountDetailsPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly MyAccountDetailsPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    private readonly Mock<IMediator> Mediator;

    public MyAccountDetailsPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.Mediator = new Mock<IMediator>();
        this.ViewModel = new MyAccountDetailsPageViewModel(this.NavigationService.Object,
                                                           this.ApplicationCache.Object,
                                                           this.DialogService.Object,
                                                           this.DeviceService.Object,
                                                           this.NavigationParameterService.Object,
                                                           this.Mediator.Object);
    }

    [Fact]
    public async Task MyAccountDetailsPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<MerchantQueries.GetMerchantDetailsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.MerchantDetailsModel));

        await this.ViewModel.Initialise(CancellationToken.None);

        this.Mediator.Verify(m => m.Send(It.IsAny<MerchantQueries.GetMerchantDetailsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        this.ViewModel.Balance.ShouldBe(TestData.Balance);
        this.ViewModel.AvailableBalance.ShouldBe(TestData.AvailableBalance);
        this.ViewModel.MerchantName.ShouldBe(TestData.MerchantName);
        this.ViewModel.LastStatementDate.ShouldBe(TestData.LastStatementDate);
        this.ViewModel.NextStatementDate.ShouldBe(TestData.NextStatementDate);
        this.ViewModel.SettlementSchedule.ShouldBe(TestData.SettlementSchedule);
    }

    [Fact]
    public async Task MyAccountDetailsPageViewModel_BackButtonCommand_PreviousPageIsShown()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}