namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ViewModels;
using ViewModels.Transactions;
using Xunit;
using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
public class BillPaymentFailedPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly BillPaymentFailedPageViewModel ViewModel;
    public BillPaymentFailedPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.ViewModel = new BillPaymentFailedPageViewModel(this.NavigationService.Object);
        Logger.Initialise(new NullLogger());
    }

    [Fact]
    public void BillPaymentFailedPageViewModel_CancelledCommand_Execute_IsExecuted()
    {
        this.ViewModel.CancelledCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}