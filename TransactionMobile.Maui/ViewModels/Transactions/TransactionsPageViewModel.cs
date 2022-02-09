namespace TransactionMobile.Maui.ViewModels.Transactions
{
    using System.Windows.Input;
    using MvvmHelpers;
    using MvvmHelpers.Commands;

    public class TransactionsPageViewModel : BaseViewModel
    {
        #region Constructors

        public TransactionsPageViewModel()
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
        }

        private async Task BillPaymentCommandExecute()
        {
        }

        private async Task MobileTopupCommandExecute()
        {
            await Shell.Current.GoToAsync(nameof(MobileTopupSelectOperatorPage));
        }

        private async Task MobileWalletCommandExecute()
        {
        }

        private async Task VoucherCommandExecute()
        {
        }

        #endregion
    }
}