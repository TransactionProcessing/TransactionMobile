namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Maui.UIServices;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Services;
using UIServices;

public class MobileTopupFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public MobileTopupFailedPageViewModel(INavigationService navigationService) {
        this.NavigationService = navigationService;
        this.CancelledCommand = new AsyncCommand(this.CancelledCommandExecute);
        this.Title = "Mobile Topup Failed";
    }

    #endregion

    #region Properties

    public ICommand CancelledCommand { get; }

    #endregion

    #region Methods

    private async Task CancelledCommandExecute()
    {
        Shared.Logger.Logger.LogInformation("CancelledCommandExecute called");
        await this.NavigationService.PopToRoot();
    }

    #endregion
}