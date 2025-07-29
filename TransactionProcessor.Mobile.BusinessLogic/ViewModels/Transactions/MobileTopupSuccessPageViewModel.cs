using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions
{
    public partial class MobileTopupSuccessPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        
        #region Constructors

        public MobileTopupSuccessPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.Title = "Mobile Topup Successful";
        }

        #endregion

        #region Methods

        [RelayCommand]
        private async Task Completed()
        {
            Logger.LogInformation("Completed called");
            await this.NavigationService.PopToRoot();
        }

        #endregion
    }
}