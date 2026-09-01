using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.Pages;

public partial class LoadingPage : ContentPage {
    private readonly IApplicationCache applicationCache;

    public LoadingPage(IApplicationCache applicationCache) {
        InitializeComponent();
        this.applicationCache = applicationCache;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
        StatusLabel.Text = "Checking authentication…";
        if (await this.isAuthenticated()) {
            await Shell.Current.GoToAsync("///home");
        }
        else {
            await Shell.Current.GoToAsync("///login");
        }
        base.OnNavigatedTo(args);
    }

    async Task<bool> isAuthenticated() {
        await Task.Delay(2000);
        Boolean isLoggedIn = this.applicationCache.GetIsLoggedIn();
        return isLoggedIn;
    }
}
