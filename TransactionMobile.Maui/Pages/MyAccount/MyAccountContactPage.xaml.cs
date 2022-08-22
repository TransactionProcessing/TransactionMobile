namespace TransactionMobile.Maui.Pages.MyAccount;

using BusinessLogic.ViewModels.MyAccount;

public partial class MyAccountContactPage : ContentPage
{
    private MyAccountContactPageViewModel viewModel => BindingContext as MyAccountContactPageViewModel;

    public MyAccountContactPage(MyAccountContactPageViewModel vm) {
        InitializeComponent();

        BindingContext = vm;

    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await viewModel.Initialise(CancellationToken.None);
    }
}