namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using Maui.UIServices;
using Moq;
using Services;
using Shared.Logger;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly MobileTopupSuccessPageViewModel ViewModel;
    public MobileTopupSuccessPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        this.ViewModel = new MobileTopupSuccessPageViewModel(this.NavigationService.Object);
    }
    
    [Fact]
    public void MobileTopupSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}