namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Logging;
using Maui.UIServices;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Services;
using UIServices;

public class MobileTopupFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    private readonly ILoggerService Logger;

    #region Constructors

    public MobileTopupFailedPageViewModel(INavigationService navigationService, ILoggerService logger) {
        this.NavigationService = navigationService;
        this.Logger = logger;
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
        await Logger.LogInformation("CancelledCommandExecute called");
        await this.NavigationService.PopToRoot();
    }

    #endregion
}