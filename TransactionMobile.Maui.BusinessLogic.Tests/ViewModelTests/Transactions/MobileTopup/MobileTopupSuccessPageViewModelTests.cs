namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.MobileTopup;

using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

public class MobileTopupSuccessPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly MobileTopupSuccessPageViewModel ViewModel;
    private readonly Mock<ILoggerService> LoggerService;
    public MobileTopupSuccessPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.LoggerService= new Mock<ILoggerService>();
        this.ViewModel = new MobileTopupSuccessPageViewModel(this.NavigationService.Object,this.LoggerService.Object);
    }
    
    [Fact]
    public void MobileTopupSuccessPageViewModel_CompletedCommand_Execute_IsExecuted()
    {
        this.ViewModel.CompletedCommand.Execute(null);
        this.NavigationService.Verify(n => n.PopToRoot(), Times.Once);
    }
}