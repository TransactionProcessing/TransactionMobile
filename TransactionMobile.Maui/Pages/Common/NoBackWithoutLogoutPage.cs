namespace TransactionMobile.Maui.Pages.Common;

using AppHome;
using BusinessLogic.ViewModels;
using MyAccount;
using Reports;
using Support;
using TransactionMobile.Maui.BusinessLogic.Services;
using TransactionMobile.Maui.BusinessLogic.UIServices;
using TransactionMobile.Maui.UIServices;
using Transactions;

public class NoBackWithoutLogoutPage : ContentPage
{
    protected override Boolean OnBackButtonPressed() {
        Type type = this.GetType().UnderlyingSystemType;

        Boolean result = false;
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
            //Logger.LogInformation("LogoutCommand called");
            applicationCache.SetAccessToken(null);

            await navigationService.GoToLoginPage();
        }
    }
}