namespace TransactionMobile.Maui.Pages.MyAccount;

using BusinessLogic.ViewModels.MyAccount;

public partial class MyAccountDetailsPage : ContentPage
{

    private MyAccountDetailsPageViewModel viewModel => BindingContext as MyAccountDetailsPageViewModel;

    public MyAccountDetailsPage(MyAccountDetailsPageViewModel vm)
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