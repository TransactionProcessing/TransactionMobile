using Imposter.Abstractions;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

[Collection("ViewModelTests")]
public class VoucherIssueSuccessPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;
    private readonly VoucherIssueSuccessPageViewModel ViewModel;

    public VoucherIssueSuccessPageViewModelTests()
    {
        this.NavigationService = new INavigationServiceImposter();
        this.ViewModel = new VoucherIssueSuccessPageViewModel(this.NavigationService.Instance());
    }

    [Fact]
    public void VoucherIssueSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.PopToRoot().Called(Count.Once());
    }
}