using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;
    
    #region Constructors

    public MobileTopupFailedPageViewModel(INavigationService navigationService) {
        this.NavigationService = navigationService;
        this.Title = "Mobile Topup Failed";
    }

    #endregion

    #region Methods

    [RelayCommand]
    private async Task Cancelled()
    {
        Logger.LogInformation("Cancelled called");
        await this.NavigationService.PopToRoot();
    }

    #endregion
}