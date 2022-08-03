namespace TransactionMobile.Maui.Pages.AppHome;

using BusinessLogic.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

public partial class HomePage : ContentPage
{
    private HomePageViewModel viewModel => BindingContext as HomePageViewModel;

    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }

}