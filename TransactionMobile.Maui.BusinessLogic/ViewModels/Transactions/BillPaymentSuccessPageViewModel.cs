namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Maui.UIServices;
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
        Shared.Logger.Logger.LogInformation("CompletedCommandExecute called");
        await this.NavigationService.PopToRoot();
    }

    #endregion
}