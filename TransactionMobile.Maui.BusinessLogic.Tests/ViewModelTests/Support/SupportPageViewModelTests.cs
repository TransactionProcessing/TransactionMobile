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
    using System.Linq;
    using System.Text;
    using Castle.Components.DictionaryAdapter;
    using Shouldly;

    [Collection("ViewModelTests")]
    public class SupportPageViewModelTests
    {
        private readonly Mock<INavigationService> NavigationService;

        private readonly Mock<INavigationParameterService> NavigationParameterService;

        private readonly Mock<IDatabaseContext> DatabaseContext;

        private readonly Mock<IMediator> Mediator;

        private readonly Mock<IDeviceService> DeviceService;

        private readonly Mock<IApplicationInfoService> ApplicationInfoService;

        private readonly Mock<IApplicationCache> ApplicationCache;

        private readonly Mock<IDialogService> DialogService;

        private readonly SupportPageViewModel ViewModel;
        public SupportPageViewModelTests() {
            this.NavigationService = new Mock<INavigationService>();
            this.NavigationParameterService = new Mock<INavigationParameterService>();
            this.DatabaseContext = new Mock<IDatabaseContext>();
            this.Mediator = new Mock<IMediator>();
            this.DeviceService = new Mock<IDeviceService>();
            this.ApplicationInfoService = new Mock<IApplicationInfoService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogService = new Mock<IDialogService>();
            this.ViewModel = new SupportPageViewModel(this.DeviceService.Object,
                                                      this.ApplicationInfoService.Object,
                                                      this.DatabaseContext.Object,
                                                      this.Mediator.Object,
                                                      this.NavigationService.Object,
                                                      this.ApplicationCache.Object,
                                                      this.DialogService.Object,
                                                      this.NavigationParameterService.Object);
        }

        [Fact]
        public void SupportPageViewModel_UploadLogsCommand_Execute_IsExecuted()
        {
            this.ViewModel.UploadLogsCommand.Execute(null);

            this.Mediator.Verify(m => m.Send(It.IsAny<UploadLogsRequest>(),It.IsAny<CancellationToken>()),Times.Once);
        }

        [Fact]
        public void SupportPageViewModel_ViewLogsCommand_Execute_IsExecuted()
        {
            this.ViewModel.ViewLogsCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoToViewLogsPage(), Times.Once);
        }

        [Fact]
        public void SupportPageViewModel_Platform_ValueIsReturned(){
            this.DeviceService.Setup(d => d.GetPlatform()).Returns("Platform");
            this.DeviceService.Setup(d => d.GetManufacturer()).Returns("Manufacturer");
            this.DeviceService.Setup(d => d.GetModel()).Returns("Model");

            String platform = this.ViewModel.Platform;

            StringBuilder expectedPlatform = new();
            expectedPlatform.Append("Platform: ").AppendLine("Platform");
            expectedPlatform.Append("Manufacturer: ").AppendLine("Manufacturer");
            expectedPlatform.Append("Device: ").AppendLine("Model");

            platform.ShouldBe(expectedPlatform.ToString());
        }

        [Fact]
        public void SupportPageViewModel_NumberTransactionsStored_ValueIsReturned()
        {
            this.DatabaseContext.Setup(d => d.GetTransactions(It.IsAny<Boolean>())).ReturnsAsync(new List<TransactionRecord>{
                                                                                                                                new TransactionRecord()

                                                                                                                            });
            String numberTransactionsStored = this.ViewModel.NumberTransactionsStored;

            String expectedNumberTransactionsStored = $"Transactions Stored: 1";

            numberTransactionsStored.ShouldBe(expectedNumberTransactionsStored);
        }

        [Fact]
        public void SupportPageViewModel_ApplicationName_ValueIsReturned()
        {
            this.ApplicationInfoService.Setup(d => d.ApplicationName).Returns("ApplicationName");
            this.ApplicationInfoService.Setup(d => d.VersionString).Returns("VersionString");

            String applicationName = this.ViewModel.ApplicationName;

            String expectedApplicationName = $"ApplicationName vVersionString";

            applicationName.ShouldBe(expectedApplicationName);
        }

        [Fact]
        public void SupportPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
