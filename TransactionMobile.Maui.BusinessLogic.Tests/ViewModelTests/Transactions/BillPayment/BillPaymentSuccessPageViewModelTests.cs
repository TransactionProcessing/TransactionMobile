namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.BillPayment;

using Maui.UIServices;
using Moq;
using Shared.Logger;
using ViewModels.Transactions;
using Xunit;

public class BillPaymentSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly BillPaymentSuccessPageViewModel ViewModel;
    public BillPaymentSuccessPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        Logger.Initialise(NullLogger.Instance);
        this.ViewModel = new BillPaymentSuccessPageViewModel(this.NavigationService.Object);
    }

    [Fact]
    public void BillPaymentSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}