namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions
{
    using System.Windows.Input;
    using Logging;
    using Maui.UIServices;
    using Microsoft.Extensions.Logging;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Services;
    using UIServices;

    public class VoucherIssueSuccessPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        private readonly ILoggerService Logger;

        #region Constructors

        public VoucherIssueSuccessPageViewModel(INavigationService navigationService, ILoggerService logger)
        {
            this.NavigationService = navigationService;
            this.Logger = logger;
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