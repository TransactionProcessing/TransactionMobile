namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Maui.UIServices;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Services;
using UIServices;

public class VoucherIssueFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public VoucherIssueFailedPageViewModel(INavigationService navigationService)
    {
        this.NavigationService = navigationService;
        this.CancelledCommand = new AsyncCommand(this.CancelledCommandExecute);
        this.Title = "Voucher Issue Failed";
    }

    #endregion

    #region Properties

    public ICommand CancelledCommand { get; }

    #endregion

    #region Methods

    private async Task CancelledCommandExecute()
    {
        Logger.LogInformation("CancelledCommandExecute called");
        await this.NavigationService.PopToRoot();
    }

    #endregion
}