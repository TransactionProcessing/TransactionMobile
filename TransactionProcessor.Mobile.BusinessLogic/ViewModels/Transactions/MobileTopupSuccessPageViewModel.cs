using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions
{
    public class MobileTopupSuccessPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        
        #region Constructors

        public MobileTopupSuccessPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
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
            Logger.LogInformation("CompletedCommandExecute called");
            await this.NavigationService.PopToRoot();
        }

        #endregion
    }
}