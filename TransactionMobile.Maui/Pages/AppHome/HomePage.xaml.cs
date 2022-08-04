namespace TransactionMobile.Maui.Pages.AppHome;

using BusinessLogic.ViewModels;

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