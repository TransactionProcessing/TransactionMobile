namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions
{
    using System.Windows.Input;
    using Maui.UIServices;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using UIServices;

    public class TransactionsPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        #region Constructors

        public TransactionsPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
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
            Shared.Logger.Logger.LogInformation("AdminCommandExecute called");
            await this.NavigationService.GoToAdminPage();
        }

        private async Task BillPaymentCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("AdminCommandExecute called");
            await this.NavigationService.GoToHome();
        }

        private async Task MobileTopupCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("MobileTopupCommandExecute called");
            await this.NavigationService.GoToMobileTopupSelectOperatorPage();
        }

        private async Task MobileWalletCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("MobileWalletCommandExecute called");
            await this.NavigationService.GoToHome();
        }

        private async Task VoucherCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("VoucherCommandExecute called");
            await this.NavigationService.GoToVoucherSelectOperatorPage();
        }

        #endregion
    }
}