﻿namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Logging;
using Maui.UIServices;
using Microsoft.Extensions.Logging;
using MvvmHelpers;
using MvvmHelpers.Commands;

public class BillPaymentFailedPageViewModel : BaseViewModel
{
    private readonly INavigationService NavigationService;

    #region Constructors

    public BillPaymentFailedPageViewModel(INavigationService navigationService)
    {
        this.NavigationService = navigationService;
        this.CancelledCommand = new AsyncCommand(this.CancelledCommandExecute);
        this.Title = "Bill Payment Failed";
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