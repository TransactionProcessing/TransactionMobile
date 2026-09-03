using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class VoucherIssueFailedPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;
    
    private readonly VoucherIssueFailedPageViewModel ViewModel;

    public VoucherIssueFailedPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
    
        this.ViewModel = new VoucherIssueFailedPageViewModel(this.NavigationService.Instance());
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void VoucherIssueFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.PopToRoot().Called(Count.Once());
    }
}