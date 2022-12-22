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
    using System.Collections.Generic;
    using System.Threading;
    using Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Requests;
    using Services;
    using ViewModels;

    public class SupportPageViewModelTests
    {
        private readonly Mock<INavigationService> NavigationService;

        private readonly Mock<IDatabaseContext> DatabaseContext;

        private readonly Mock<IMediator> Mediator;

        private readonly Mock<IDeviceService> DeviceService;

        private readonly Mock<IApplicationInfoService> ApplicationInfoService;

        private readonly Mock<IApplicationCache> ApplicationCache;

        private readonly Mock<IDialogService> DialogService;

        private readonly SupportPageViewModel ViewModel;
        private readonly Mock<ILoggerService> LoggerService;
        public SupportPageViewModelTests() {
            this.NavigationService = new Mock<INavigationService>();
            this.DatabaseContext = new Mock<IDatabaseContext>();
            this.Mediator = new Mock<IMediator>();
            this.DeviceService = new Mock<IDeviceService>();
            this.ApplicationInfoService = new Mock<IApplicationInfoService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogService = new Mock<IDialogService>();
            this.LoggerService = new Mock<ILoggerService>();
            this.ViewModel = new SupportPageViewModel(this.DeviceService.Object,
                                                      this.ApplicationInfoService.Object,
                                                      this.DatabaseContext.Object,
                                                      this.Mediator.Object,
                                                      this.NavigationService.Object,
                                                      this.ApplicationCache.Object,
                                                      this.DialogService.Object,
                                                      this.LoggerService.Object);
        }

        [Fact]
        public void SupportPageViewModel_UploadLogsCommand_Execute_IsExecuted()
        {
            this.ViewModel.UploadLogsCommand.Execute(null);

            this.Mediator.Verify(m => m.Send(It.IsAny<UploadLogsRequest>(),It.IsAny<CancellationToken>()),Times.Once);
            this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        }

        [Fact]
        public void SupportPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
