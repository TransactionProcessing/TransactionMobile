using MediatR;
using Imposter.Abstractions;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Admin;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Transactions.Admin
{
    [Collection("ViewModelTests")]
    public class AdminPageViewModelTests
    {
        private readonly INavigationServiceImposter NavigationService;
        private readonly INavigationParameterServiceImposter NavigationParameterService;

        private readonly IMediatorImposter Mediator;

        private readonly IDeviceServiceImposter DeviceService;

        private readonly IApplicationInfoServiceImposter ApplicationInfoService;

        private readonly IApplicationCacheImposter ApplicationCache;

        private readonly IDialogServiceImposter DialogService;

        private readonly AdminPageViewModel ViewModel;
        public AdminPageViewModelTests() {
            this.NavigationService = new INavigationServiceImposter();
            this.NavigationParameterService = new INavigationParameterServiceImposter();
            this.Mediator = new IMediatorImposter();
            this.DeviceService = new IDeviceServiceImposter();
            this.ApplicationInfoService = new IApplicationInfoServiceImposter();
            this.DialogService = new IDialogServiceImposter();
            this.ApplicationCache = new IApplicationCacheImposter();
            this.ViewModel = new AdminPageViewModel(this.Mediator.Instance(),
                                                    this.NavigationService.Instance(),
                                                    this.ApplicationCache.Instance(),
                                                    this.DialogService.Instance(),
                                                    this.DeviceService.Instance(),
                                                    this.ApplicationInfoService.Instance(), this.NavigationParameterService.Instance());
        }

        [Fact]
        public void AdminPageViewModel_AdminCommand_Execute_IsExecuted()
        {
            this.Mediator.Send(Arg<IRequest<Result<PerformReconciliationResponseModel>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
            this.ViewModel.ReconciliationCommand.Execute(null);
            this.NavigationService.GoToHome().Called(Count.Once());
        }

        [Fact]
        public void AdminPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            this.ViewModel.BackButtonCommand.Execute(null);
            this.NavigationService.GoBack().Called(Count.Once());
        }
    }
}
