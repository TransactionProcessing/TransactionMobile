using Moq;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using NullLogger = Logging.NullLogger;

[Collection("ViewModelTests")]
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