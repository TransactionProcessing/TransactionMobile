namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ViewModels;
using ViewModels.Transactions;
using Xunit;
using NullLogger = Logging.NullLogger;

public class BillPaymentSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly BillPaymentSuccessPageViewModel ViewModel;
    public BillPaymentSuccessPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        this.ViewModel = new BillPaymentSuccessPageViewModel(this.NavigationService.Object);
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void BillPaymentSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}