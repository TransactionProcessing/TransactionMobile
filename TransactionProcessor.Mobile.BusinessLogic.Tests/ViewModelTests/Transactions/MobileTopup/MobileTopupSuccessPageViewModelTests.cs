using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

[Collection("ViewModelTests")]
public class MobileTopupSuccessPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;

    private readonly MobileTopupSuccessPageViewModel ViewModel;
    
    public MobileTopupSuccessPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
        
        this.ViewModel = new MobileTopupSuccessPageViewModel(this.NavigationService.Instance());
    }
    
    [Fact]
    public void MobileTopupSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.PopToRoot().Called(Count.Once());
    }
}