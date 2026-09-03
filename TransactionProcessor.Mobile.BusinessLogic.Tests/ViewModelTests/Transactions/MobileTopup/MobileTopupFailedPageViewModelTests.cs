using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class MobileTopupFailedPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;

    private readonly MobileTopupFailedPageViewModel ViewModel;
    public MobileTopupFailedPageViewModelTests()
    {
        this.NavigationService = new INavigationServiceImposter();

        this.ViewModel = new MobileTopupFailedPageViewModel(this.NavigationService.Instance());
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void MobileTopupFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.PopToRoot().Called(Count.Once());
    }
}