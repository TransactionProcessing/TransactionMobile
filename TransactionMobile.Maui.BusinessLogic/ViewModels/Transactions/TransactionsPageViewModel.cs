﻿namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions
{
    using System.Windows.Input;
    using Logging;
    using Maui.UIServices;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Services;
    using UIServices;

    public class TransactionsPageViewModel : ExtendedBaseViewModel
    {
        #region Constructors

        public TransactionsPageViewModel(INavigationService navigationService,
                                         IApplicationCache applicationCache,
                                         IDialogService dialogService, ILoggerService logger) : base(applicationCache, dialogService, navigationService, logger)
        {
            this.MobileTopupCommand = new AsyncCommand(this.MobileTopupCommandExecute);
            this.MobileWalletCommand = new AsyncCommand(this.MobileWalletCommandExecute);
            this.BillPaymentCommand = new AsyncCommand(this.BillPaymentCommandExecute);
            this.VoucherCommand = new AsyncCommand(this.VoucherCommandExecute);
            this.AdminCommand = new AsyncCommand(this.AdminCommandExecute);
            this.Title = "Select Transaction Type";
        }

        #endregion

        #region Properties

        public ICommand AdminCommand { get; set; }

        public ICommand BillPaymentCommand { get; set; }

        public ICommand MobileTopupCommand { get; set; }

        public ICommand MobileWalletCommand { get; set; }

        public ICommand VoucherCommand { get; set; }

        #endregion

        #region Methods

        private async Task AdminCommandExecute()
        {
            await Logger.LogInformation("AdminCommandExecute called");
            await this.NavigationService.GoToAdminPage();
        }

        private async Task BillPaymentCommandExecute()
        {
            await Logger.LogInformation("AdminCommandExecute called");
            await this.NavigationService.GoToBillPaymentSelectOperatorPage();
        }

        private async Task MobileTopupCommandExecute()
        {
            await Logger.LogInformation("MobileTopupCommandExecute called");
            await this.NavigationService.GoToMobileTopupSelectOperatorPage();
        }

        private async Task MobileWalletCommandExecute()
        {
            await Logger.LogInformation("MobileWalletCommandExecute called");
            await this.NavigationService.GoToHome();
        }

        private async Task VoucherCommandExecute()
        {
            await Logger.LogInformation("VoucherCommandExecute called");
            await this.NavigationService.GoToVoucherSelectOperatorPage();
        }

        #endregion
    }
}