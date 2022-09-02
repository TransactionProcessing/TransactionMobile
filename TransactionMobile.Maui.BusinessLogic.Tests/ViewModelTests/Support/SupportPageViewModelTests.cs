using System;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Support
{
    using Maui.UIServices;
    using MediatR;
    using Moq;
    using UIServices;
    using Xunit;
    using TransactionMobile.Maui.BusinessLogic.ViewModels.Support;
    using TransactionMobile.Maui.Database;
    using Shared.Logger;
    using System.Collections.Generic;
    using System.Threading;
    using Requests;
    using Services;

    public class SupportPageViewModelTests
    {
        [Fact]
        public void SupportPageViewModel_UploadLogsCommand_Execute_IsExecuted()
        {
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
            Mock<IMediator> mediator = new Mock<IMediator>();
            Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
            Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
            Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
            Mock<IDialogService> dialogService = new Mock<IDialogService>();
            Logger.Initialise(NullLogger.Instance);
            SupportPageViewModel viewModel = new SupportPageViewModel(deviceService.Object,
                                                                      applicationInfoService.Object,
                                                                      databaseContext.Object,
                                                                      mediator.Object,
                                                                      navigationService.Object,
                                                                      applicationCache.Object,
                                                                      dialogService.Object);

            viewModel.UploadLogsCommand.Execute(null);

            mediator.Verify(m => m.Send(It.IsAny<UploadLogsRequest>(),It.IsAny<CancellationToken>()),Times.Once);
            navigationService.Verify(n => n.GoToHome(), Times.Once);
        }

        [Fact]
        public void SupportPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
            Mock<IMediator> mediator = new Mock<IMediator>();
            Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
            Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
            Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
            Mock<IDialogService> dialogService = new Mock<IDialogService>();
            Logger.Initialise(NullLogger.Instance);
            SupportPageViewModel viewModel = new SupportPageViewModel(deviceService.Object,
                                                                      applicationInfoService.Object,
                                                                      databaseContext.Object,
                                                                      mediator.Object,
                                                                      navigationService.Object,
                                                                      applicationCache.Object,
                                                                      dialogService.Object);

            viewModel.BackButtonCommand.Execute(null);

            navigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
