using Microsoft.Maui.Controls;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.Pages;

public partial class LoadingPage : ContentPage {
    public LoadingPage() {
        InitializeComponent();

    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
        if (await this.isAuthenticated()) {
            await Shell.Current.GoToAsync("///home");
        }
        else {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
    }

    async Task<bool> isAuthenticated() {
        await Task.Delay(2000);
        IApplicationCache applicationCache = MauiProgram.Container.Services.GetService<IApplicationCache>();
        Boolean isLoggedIn = applicationCache.GetIsLoggedIn();
        return isLoggedIn;
    }
}