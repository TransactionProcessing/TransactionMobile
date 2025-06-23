using Moq;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

[Collection("ViewModelTests")]
public class VoucherIssueSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly VoucherIssueSuccessPageViewModel ViewModel;

    public VoucherIssueSuccessPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        this.ViewModel = new VoucherIssueSuccessPageViewModel(this.NavigationService.Object);
    }

    [Fact]
    public void VoucherIssueSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}