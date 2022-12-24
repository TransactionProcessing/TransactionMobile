namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions
{
    using System.Windows.Input;
    using Logging;
    using Maui.UIServices;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Services;
    using UIServices;

    public class MobileTopupSuccessPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        private readonly ILoggerService Logger;

        #region Constructors

        public MobileTopupSuccessPageViewModel(INavigationService navigationService, ILoggerService logger)
        {
            this.NavigationService = navigationService;
            this.Logger = logger;
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
            await Logger.LogInformation("CompletedCommandExecute called");
            await this.NavigationService.PopToRoot();
        }

        #endregion
    }
}