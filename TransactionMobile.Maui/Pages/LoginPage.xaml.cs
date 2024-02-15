namespace TransactionMobile.Maui.Pages;

using BusinessLogic.Logging;
using BusinessLogic.ViewModels;

public partial class LoginPage : ContentPage
{
	private LoginPageViewModel viewModel => BindingContext as LoginPageViewModel;

	public LoginPage(LoginPageViewModel vm)
	{
        Logger.LogInformation("LoginPage ctor");

        InitializeComponent();
        BindingContext = vm;
    }

	protected override async void OnAppearing()
	{
		base.OnAppearing();
	}

    protected override bool OnBackButtonPressed() {
        Application.Current.Quit();
        return true;
    }
}