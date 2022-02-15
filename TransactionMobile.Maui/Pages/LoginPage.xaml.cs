namespace TransactionMobile.Maui.Pages;

using BusinessLogic.ViewModels;

public partial class LoginPage : ContentPage
{
	private LoginPageViewModel viewModel => BindingContext as LoginPageViewModel;

	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		//await viewModel.InitializeAsync();
	}
}