namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Support;

using Maui.UIServices;
using MediatR;
using Moq;
using Services;
using Shared.Logger;
using UIServices;
using ViewModels.Support;
using Xunit;

public class ViewLogsPageViewModelTests
{
    [Fact]
    public void ViewLogsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IMediator> mediator = new Mock<IMediator>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        Mock<IDialogService> dialogService = new Mock<IDialogService>();
        Logger.Initialise(NullLogger.Instance);
        ViewLogsPageViewModel viewModel = new ViewLogsPageViewModel(mediator.Object,
                                                                    navigationService.Object,
                                                                    applicationCache.Object,
                                                                    dialogService.Object);

        viewModel.BackButtonCommand.Execute(null);

        navigationService.Verify(n => n.GoBack(), Times.Once);
    }
}