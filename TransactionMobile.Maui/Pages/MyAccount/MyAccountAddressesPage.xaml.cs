namespace TransactionMobile.Maui.Pages.MyAccount;

using BusinessLogic.ViewModels.MyAccount;

public partial class MyAccountAddressesPage : ContentPage
{
    private MyAccountAddressPageViewModel viewModel => BindingContext as MyAccountAddressPageViewModel;

    public MyAccountAddressesPage(MyAccountAddressPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.Initialise(CancellationToken.None);
    }
}