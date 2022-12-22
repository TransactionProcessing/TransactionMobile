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

public class VoucherIssueSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<ILoggerService> LoggerService;
    private readonly VoucherIssueSuccessPageViewModel ViewModel;

    public VoucherIssueSuccessPageViewModelTests()
    {
        NavigationService = new Mock<INavigationService>();
        LoggerService = new Mock<ILoggerService>();
        this.ViewModel = new VoucherIssueSuccessPageViewModel(this.NavigationService.Object, this.LoggerService.Object);
    }

    [Fact]
    public void VoucherIssueSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}