using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class VoucherIssueFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public VoucherIssueFailedPageViewModel(INavigationService navigationService)
    {
        this.NavigationService = navigationService;
        this.Title = "Voucher Issue Failed";
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