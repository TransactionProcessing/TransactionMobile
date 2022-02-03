using TransactionMobile.Maui.ViewModels;

namespace TransactionMobile.Maui.Pages;

public partial class LoginPage : ContentPage
{
	private LoginViewModel viewModel => BindingContext as LoginViewModel;

	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await viewModel.InitializeAsync();
	}
}