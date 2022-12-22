namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

public class VoucherIssueFailedPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly VoucherIssueFailedPageViewModel ViewModel;

    public VoucherIssueFailedPageViewModelTests() {
        NavigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        this.ViewModel = new VoucherIssueFailedPageViewModel(this.NavigationService.Object);
    }

    [Fact]
    public void VoucherIssueFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}