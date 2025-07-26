using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public BillPaymentFailedPageViewModel(INavigationService navigationService)
    {
        this.NavigationService = navigationService;
        this.Title = "Bill Payment Failed";
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