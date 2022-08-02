namespace TransactionMobile.Maui.Pages.MyAccount;

using BusinessLogic.ViewModels.MyAccount;

public partial class MyAccountPage : ContentPage
{
    private MyAccountPageViewModel viewModel => BindingContext as MyAccountPageViewModel;

    public MyAccountPage(MyAccountPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }

}