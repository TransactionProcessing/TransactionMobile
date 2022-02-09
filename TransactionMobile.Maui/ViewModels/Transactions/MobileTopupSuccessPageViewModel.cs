namespace TransactionMobile.Maui.ViewModels.Transactions
{
    using System.Windows.Input;
    using MvvmHelpers;
    using MvvmHelpers.Commands;

    public class MobileTopupSuccessPageViewModel : BaseViewModel
    {
        #region Constructors

        public MobileTopupSuccessPageViewModel()
        {
            this.CompletedCommand = new AsyncCommand(this.CompletedCommandExecute);
            this.Title = "Mobile Topup Successful";
        }

        #endregion

        #region Properties

        public ICommand CompletedCommand { get; }

        #endregion

        #region Methods

        private async Task CompletedCommandExecute()
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }

        #endregion
    }
}