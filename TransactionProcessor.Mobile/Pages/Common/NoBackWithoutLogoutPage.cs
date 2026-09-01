using Microsoft.Maui.Controls;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.Pages.AppHome;
using TransactionProcessor.Mobile.Pages.MyAccount;
using TransactionProcessor.Mobile.Pages.Reports;
using TransactionProcessor.Mobile.Pages.Support;
using TransactionProcessor.Mobile.Pages.Transactions;

namespace TransactionProcessor.Mobile.Pages.Common;

public class NoBackWithoutLogoutPage : ContentPage
{
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly IApplicationCache applicationCache;

    protected NoBackWithoutLogoutPage(INavigationService navigationService,
                                      IDialogService dialogService,
                                      IApplicationCache applicationCache)
    {
        this.navigationService = navigationService;
        this.dialogService = dialogService;
        this.applicationCache = applicationCache;
    }

    protected override Boolean OnBackButtonPressed() {
        Type type = this.GetType().UnderlyingSystemType;

        this.Dispatcher.Dispatch(async () => {
                                     Task t = type.Name switch {
                                         nameof(TransactionsPage) => this.ShowHomePage(),
                                         nameof(MyAccountPage) => this.ShowHomePage(),
                                         nameof(ReportsPage) => this.ShowHomePage(),
                                         nameof(SupportPage) => this.ShowHomePage(),
                                         nameof(HomePage) => this.ShowLoginPage()
                                     };
                                     await t;
                                 });
        return true;
    }

    private async Task ShowHomePage() {
        await this.navigationService.GoToHome();
    }

    private async Task ShowLoginPage()
    {
        Boolean leave = await this.dialogService.ShowDialog("Title", "Logout Message", "yes", "no");
        if (leave)
        {
            this.applicationCache.SetIsLoggedIn(false);
            this.applicationCache.ClearAccessToken();
            this.applicationCache.ClearMerchantDetails();

            await this.navigationService.GoToLoginPage();
        }
    }
}
