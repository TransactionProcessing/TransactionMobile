using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Admin
{
    using Logging;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using UIServices;
    using ViewModels.Admin;
    using ViewModels.Transactions;
    using Xunit;
    using TransactionMobile.Maui.BusinessLogic.Services;

    [Collection("ViewModelTests")]
    public class AdminPageViewModelTests
    {
        private readonly Mock<INavigationService> NavigationService;
        private readonly Mock<INavigationParameterService> NavigationParameterService;

        private readonly Mock<IMediator> Mediator;

        private readonly Mock<IDeviceService> DeviceService;

        private readonly Mock<IApplicationInfoService> ApplicationInfoService;

        private readonly Mock<IApplicationCache> ApplicationCache;

        private readonly Mock<IDialogService> DialogService;

        private readonly AdminPageViewModel ViewModel;
        public AdminPageViewModelTests() {
            this.NavigationService = new Mock<INavigationService>();
            this.NavigationParameterService = new Mock<INavigationParameterService>();
            this.Mediator = new Mock<IMediator>();
            this.DeviceService = new Mock<IDeviceService>();
            this.ApplicationInfoService = new Mock<IApplicationInfoService>();
            this.DialogService = new Mock<IDialogService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.ViewModel = new AdminPageViewModel(this.Mediator.Object,
                                                    this.NavigationService.Object,
                                                    this.ApplicationCache.Object,
                                                    this.DialogService.Object,
                                                    this.DeviceService.Object,
                                                    this.ApplicationInfoService.Object, this.NavigationParameterService.Object);
        }

        [Fact]
        public void AdminPageViewModel_AdminCommand_Execute_IsExecuted()
        {
            this.ViewModel.ReconciliationCommand.Execute(null);
            this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        }

        [Fact]
        public void AdminPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            this.ViewModel.BackButtonCommand.Execute(null);
            this.NavigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }
}
