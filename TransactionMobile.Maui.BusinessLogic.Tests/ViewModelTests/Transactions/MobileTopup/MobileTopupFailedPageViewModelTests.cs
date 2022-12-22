namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupFailedPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly MobileTopupFailedPageViewModel ViewModel;
    public MobileTopupFailedPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        this.ViewModel = new MobileTopupFailedPageViewModel(this.NavigationService.Object);
    }

    [Fact]
    public void MobileTopupFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}