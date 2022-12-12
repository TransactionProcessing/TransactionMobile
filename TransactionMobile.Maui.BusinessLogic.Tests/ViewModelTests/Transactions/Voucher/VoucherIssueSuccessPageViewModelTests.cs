namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using Maui.UIServices;
using Moq;
using Services;
using Shared.Logger;
using UIServices;
using ViewModels.Transactions;
using Xunit;

public class VoucherIssueSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly VoucherIssueSuccessPageViewModel ViewModel;

    public VoucherIssueSuccessPageViewModelTests()
    {
        NavigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        this.ViewModel = new VoucherIssueSuccessPageViewModel(this.NavigationService.Object);
    }

    [Fact]
    public void VoucherIssueSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}