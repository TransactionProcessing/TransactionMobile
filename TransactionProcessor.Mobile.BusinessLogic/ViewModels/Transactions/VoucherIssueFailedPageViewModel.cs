using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

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