namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions
{
    using System.Windows.Input;
    using Maui.UIServices;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Services;
    using UIServices;

    public class VoucherIssueSuccessPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        #region Constructors

        public VoucherIssueSuccessPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.CompletedCommand = new AsyncCommand(this.CompletedCommandExecute);
            this.Title = "Voucher Issue Successful";
        }

        #endregion

        #region Properties

        public ICommand CompletedCommand { get; }

        #endregion

        #region Methods

        private async Task CompletedCommandExecute()
        {
            Logger.LogInformation("CompletedCommandExecute called");
            await this.NavigationService.PopToRoot();
        }

        #endregion
    }
}