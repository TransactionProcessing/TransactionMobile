using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests
{
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using UIServices;
    using ViewModels.Admin;
    using ViewModels.Transactions;
    using Xunit;

    public class AdminPageViewModelTests
    {
        [Fact]
        public void TransactionsPageViewModel_AdminCommand_Execute_IsExecuted()
        {
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IMediator> mediator = new Mock<IMediator>();
            Mock<IMemoryCache> userDetailsCache = new Mock<IMemoryCache>();
            Mock<IMemoryCache> configurationCache = new Mock<IMemoryCache>();
            Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
            Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
            AdminPageViewModel viewModel = new AdminPageViewModel(mediator.Object,
                                                                  navigationService.Object,
                                                                  userDetailsCache.Object,
                                                                  configurationCache.Object,
                                                                  deviceService.Object,
                                                                  applicationInfoService.Object);

            viewModel.ReconciliationCommand.Execute(null);
            navigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
