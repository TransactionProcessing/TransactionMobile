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
        INavigationService navigationService = MauiProgram.Container.Services.GetRequiredService<INavigationService>();
        await navigationService.GoToHome();
    }

    private async Task ShowLoginPage()
    {
        IDialogService dialogService = MauiProgram.Container.Services.GetRequiredService<IDialogService>();
        IApplicationCache applicationCache = MauiProgram.Container.Services.GetRequiredService<IApplicationCache>();
        INavigationService navigationService = MauiProgram.Container.Services.GetRequiredService<INavigationService>();

        Boolean leave = await dialogService.ShowDialog("Title", "Logout Message", "yes", "no");
        if (leave)
        {
            applicationCache.SetIsLoggedIn(false);
            applicationCache.SetAccessToken(null);

            await navigationService.GoToLoginPage();
        }
    }
}