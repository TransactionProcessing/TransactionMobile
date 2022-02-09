using TransactionMobile.Maui.ViewModels;

namespace TransactionMobile.Maui.Pages;

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