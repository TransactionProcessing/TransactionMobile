using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using MvvmHelpers;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentSuccessPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public BillPaymentSuccessPageViewModel(INavigationService navigationService)
    {
        this.NavigationService = navigationService;
        this.Title = "Bill Payment Successful";
    }

    #endregion

    #region Methods

    [RelayCommand]
    private async Task Completed()
    {
        Logger.LogInformation("CompletedCommand called");
        await this.NavigationService.PopToRoot();
    }

    #endregion
}