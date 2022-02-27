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
            await this.NavigationService.GoToHome();
        }

        private async Task BillPaymentCommandExecute()
        {
            await this.NavigationService.GoToHome();
        }

        private async Task MobileTopupCommandExecute()
        {
            await this.NavigationService.GoToMobileTopupSelectOperatorPage();
        }

        private async Task MobileWalletCommandExecute()
        {
            await this.NavigationService.GoToHome();
        }

        private async Task VoucherCommandExecute()
        {
            await this.NavigationService.GoToVoucherSelectOperatorPage();
        }

        #endregion
    }
}