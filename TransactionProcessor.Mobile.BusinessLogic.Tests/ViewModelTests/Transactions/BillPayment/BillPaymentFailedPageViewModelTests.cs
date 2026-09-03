using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class BillPaymentFailedPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;
    private readonly BillPaymentFailedPageViewModel ViewModel;
    public BillPaymentFailedPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
        this.ViewModel = new BillPaymentFailedPageViewModel(this.NavigationService.Instance());
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void BillPaymentFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.PopToRoot().Called(Count.Once());
    }
}