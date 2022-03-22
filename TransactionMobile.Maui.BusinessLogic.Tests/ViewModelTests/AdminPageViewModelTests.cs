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
    using TransactionMobile.Maui.BusinessLogic.Services;

    public class AdminPageViewModelTests
    {
        [Fact]
        public void AdminPageViewModel_AdminCommand_Execute_IsExecuted()
        {
            Mock<INavigationService> navigationService = new Mock<INavigationService>();
            Mock<IMediator> mediator = new Mock<IMediator>();
            Mock<IMemoryCacheService> memoryCacheService = new Mock<IMemoryCacheService>();
            Mock<IDeviceService> deviceService = new Mock<IDeviceService>();
            Mock<IApplicationInfoService> applicationInfoService = new Mock<IApplicationInfoService>();
            AdminPageViewModel viewModel = new AdminPageViewModel(mediator.Object,
                                                                  navigationService.Object,
                                                                  memoryCacheService.Object,
                                                                  deviceService.Object,
                                                                  applicationInfoService.Object);

            viewModel.ReconciliationCommand.Execute(null);
            navigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
