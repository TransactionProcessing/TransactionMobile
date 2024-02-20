namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Admin
{
    using System.Windows.Input;
    using Common;
    using Logging;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Models;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using RequestHandlers;
    using Requests;
    using Services;
    using TransactionProcessorACL.DataTransferObjects.Responses;
    using UIServices;

    public class AdminPageViewModel : ExtendedBaseViewModel
    {
        private readonly IMediator Mediator;
        
        private readonly IApplicationInfoService ApplicationInfoService;

        #region Constructors

        public AdminPageViewModel(IMediator mediator, INavigationService navigationService,
                                  IApplicationCache applicationCache,
                                  IDialogService dialogService,
                                  IDeviceService deviceService, IApplicationInfoService applicationInfoService) :
            base(applicationCache,dialogService, navigationService, deviceService)
        {
            this.Mediator = mediator;
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
                PerformReconciliationRequest.Create(DateTime.Now, String.Empty, this.ApplicationInfoService.VersionString);

            Result<PerformReconciliationResponseModel> result = await this.Mediator.Send(request);

            // TODO: Act on the response (display message or something)...
            await this.NavigationService.GoToHome();
        }
        
        #endregion
    }
}