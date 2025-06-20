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

        //this.AddressLine2.IsVisible = String.IsNullOrEmpty(viewModel.Address.AddressLine2) switch {
        //    true => false,
        //    false => true
        //};
        //this.AddressLine3.IsVisible = String.IsNullOrEmpty(viewModel.Address.AddressLine3) switch
        //{
        //    true => false,
        //    false => true
        //};
        //this.AddressLine4.IsVisible = String.IsNullOrEmpty(viewModel.Address.AddressLine4) switch
        //{
        //    true => false,
        //    false => true
        //};
    }
}