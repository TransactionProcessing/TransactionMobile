using MediatR;
using Moq;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Admin;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.Admin
{
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
