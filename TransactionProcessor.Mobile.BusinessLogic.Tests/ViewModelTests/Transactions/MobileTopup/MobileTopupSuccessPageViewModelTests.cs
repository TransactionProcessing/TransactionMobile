using Moq;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

[Collection("ViewModelTests")]
public class MobileTopupSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly MobileTopupSuccessPageViewModel ViewModel;
    
    public MobileTopupSuccessPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        
        this.ViewModel = new MobileTopupSuccessPageViewModel(this.NavigationService.Object);
    }
    
    [Fact]
    public void MobileTopupSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}