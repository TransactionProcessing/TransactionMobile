namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging;
using MvvmHelpers;
using MvvmHelpers.Commands;

public class BillPaymentSuccessPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public BillPaymentSuccessPageViewModel(INavigationService navigationService)
    {
        this.NavigationService = navigationService;
        this.CompletedCommand = new AsyncCommand(this.CompletedCommandExecute);
        this.Title = "Bill Payment Successful";
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