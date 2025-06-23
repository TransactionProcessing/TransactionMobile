using Microsoft.Maui.Controls;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;

namespace TransactionProcessor.Mobile.Pages;

public partial class LoginPage : ContentPage
{
	private LoginPageViewModel viewModel => this.BindingContext as LoginPageViewModel;

	public LoginPage(LoginPageViewModel vm)
	{
        Logger.LogInformation("LoginPage ctor");

        InitializeComponent();
        this.BindingContext = vm;
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