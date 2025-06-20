using MediatR;
using Moq;
using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Support;

[Collection("ViewModelTests")]
public class ViewLogsPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<INavigationParameterService> NavigationParameterService;

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly ViewLogsPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public ViewLogsPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.Mediator = new Mock<IMediator>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new ViewLogsPageViewModel(this.Mediator.Object,
                                                   this.NavigationService.Object,
                                                   this.ApplicationCache.Object,
                                                   this.DialogService.Object,
                                                   this.DeviceService.Object, this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task SupportPageViewModel_UploadLogsCommand_Execute_IsExecuted(){

        this.Mediator.Setup(m => m.Send<List<LogMessage>>(It.IsAny<ViewLogsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<LogMessage>(){
                                                                                                                                                              new LogMessage()
                                                                                                                                                          });

        await this.ViewModel.LoadLogMessages();
        this.ViewModel.LogMessages.Count.ShouldBe(1);
    }

    [Fact]
    public void ViewLogsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(n => n.GoBack(), Times.Once);
    }
}