using Moq;
using Shouldly;
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

    public MyAccountDetailsPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new MyAccountDetailsPageViewModel(this.NavigationService.Object,
                                                           this.ApplicationCache.Object,
                                                           this.DialogService.Object,
                                                           this.DeviceService.Object, this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task MyAccountDetailsPageViewModel_Initialise_IsInitialised()
    {
        this.ApplicationCache.Setup(a => a.GetMerchantDetails()).Returns(TestData.MerchantDetailsModel);

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ApplicationCache.Verify(a => a.GetMerchantDetails(), Times.Once);
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