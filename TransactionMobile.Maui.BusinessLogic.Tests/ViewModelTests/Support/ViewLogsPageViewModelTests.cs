namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Support;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Models;
using Moq;
using Requests;
using Services;
using Shouldly;
using UIServices;
using ViewModels;
using ViewModels.Support;
using Xunit;

[Collection("ViewModelTests")]
public class ViewLogsPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;

    private readonly Mock<IMediator> Mediator;

    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogService;

    private readonly ViewLogsPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public ViewLogsPageViewModelTests() {
        this.NavigationService = new Mock<INavigationService>();
        this.Mediator = new Mock<IMediator>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new ViewLogsPageViewModel(this.Mediator.Object,
                                                   this.NavigationService.Object,
                                                   this.ApplicationCache.Object,
                                                   this.DialogService.Object,
                                                   this.DeviceService.Object);
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