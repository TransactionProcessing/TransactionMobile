using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

namespace TransactionProcessor.Mobile.Pages.MyAccount;

public partial class MyAccountAddressesPage : ContentPage
{
    private MyAccountAddressPageViewModel viewModel => this.BindingContext as MyAccountAddressPageViewModel;

    public MyAccountAddressesPage(MyAccountAddressPageViewModel vm)
    {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }
}