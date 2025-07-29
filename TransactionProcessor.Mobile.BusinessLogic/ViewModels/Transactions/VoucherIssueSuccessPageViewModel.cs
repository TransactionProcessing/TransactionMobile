using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions
{
    public partial class VoucherIssueSuccessPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        
        #region Constructors

        public VoucherIssueSuccessPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.Title = "Voucher Issue Successful";
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