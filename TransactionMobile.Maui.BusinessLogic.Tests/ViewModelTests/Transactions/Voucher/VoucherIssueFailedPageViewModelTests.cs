namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;
using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class VoucherIssueFailedPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    
    private readonly VoucherIssueFailedPageViewModel ViewModel;

    public VoucherIssueFailedPageViewModelTests() {
        NavigationService = new Mock<INavigationService>();
    
        this.ViewModel = new VoucherIssueFailedPageViewModel(this.NavigationService.Object);
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void VoucherIssueFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}