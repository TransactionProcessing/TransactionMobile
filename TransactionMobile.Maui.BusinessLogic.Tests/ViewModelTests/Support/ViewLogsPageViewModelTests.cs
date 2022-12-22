namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Support;

using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using UIServices;
using ViewModels;
using ViewModels.Support;
using Xunit;

public class ViewLogsPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly ViewLogsPageViewModel ViewModel;

    public ViewLogsPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.Mediator = new Mock<IMediator>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        
        Logger.Initialise(NullLogger.Instance);
        this.ViewModel = new ViewLogsPageViewModel(this.Mediator.Object,
                                                   this.NavigationService.Object,
                                                   this.ApplicationCache.Object,
                                                   this.DialogService.Object);
    }

    [Fact]
    public void ViewLogsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}