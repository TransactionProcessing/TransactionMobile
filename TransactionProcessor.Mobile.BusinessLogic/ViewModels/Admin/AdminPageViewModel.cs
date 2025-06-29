﻿using System.Windows.Input;
using MediatR;
using MvvmHelpers.Commands;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Admin
{
    public class AdminPageViewModel : ExtendedBaseViewModel
    {
        private readonly IMediator Mediator;
        
        private readonly IApplicationInfoService ApplicationInfoService;

        #region Constructors

        public AdminPageViewModel(IMediator mediator, INavigationService navigationService,
                                  IApplicationCache applicationCache,
                                  IDialogService dialogService,
                                  IDeviceService deviceService, IApplicationInfoService applicationInfoService,
                                  INavigationParameterService navigationParameterService) :
            base(applicationCache,dialogService, navigationService, deviceService, navigationParameterService)
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