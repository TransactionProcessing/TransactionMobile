using Microsoft.Maui.Controls;

namespace TransactionMobile.Maui.Pages.Transactions.Admin;

using BusinessLogic.ViewModels.Admin;

public partial class AdminPage : ContentPage
{
    private AdminPageViewModel viewModel => BindingContext as AdminPageViewModel;

    public AdminPage(AdminPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}