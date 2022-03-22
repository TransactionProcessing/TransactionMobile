using System;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests
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
            Logger.Initialise(NullLogger.Instance);
            SupportPageViewModel viewModel = new SupportPageViewModel(deviceService.Object,applicationInfoService.Object,
                databaseContext.Object, mediator.Object,navigationService.Object);

            viewModel.UploadLogsCommand.Execute(null);
            
            navigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
