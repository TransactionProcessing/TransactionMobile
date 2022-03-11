namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Admin
{
    using System.Windows.Input;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Requests;
    using UIServices;

    public class AdminPageViewModel : BaseViewModel
    {
        private readonly IMediator Mediator;

        private readonly INavigationService NavigationService;

        private readonly IMemoryCache UserDetailsCache;

        private readonly IMemoryCache ConfigurationCache;

        private readonly IDeviceService DeviceService;

        private readonly IApplicationInfoService ApplicationInfoService;

        #region Constructors

        public AdminPageViewModel(IMediator mediator, INavigationService navigationService,
                                  IMemoryCache userDetailsCache, IMemoryCache configurationCache,
                                  IDeviceService deviceService, IApplicationInfoService applicationInfoService)
        {
            this.Mediator = mediator;
            this.NavigationService = navigationService;
            this.UserDetailsCache = userDetailsCache;
            this.ConfigurationCache = configurationCache;
            this.DeviceService = deviceService;
            this.ApplicationInfoService = applicationInfoService;
            this.ReconciliationCommand = new AsyncCommand(this.ReconciliationCommandExecute);
            this.Title = "Select Admin Transaction Type";
        }

        #endregion

        #region Properties

        public ICommand ReconciliationCommand { get; set; }

        #endregion

        #region Methods

        private async Task ReconciliationCommandExecute()
        {
            PerformReconciliationRequest request =
                PerformReconciliationRequest.Create(DateTime.Now, this.DeviceService.GetIdentifier(), this.ApplicationInfoService.VersionString);

            Boolean response = await this.Mediator.Send(request);

            // TODO: Act on the response (display message or something)...
            await this.NavigationService.GoToHome();
        }

        

        #endregion
    }
}