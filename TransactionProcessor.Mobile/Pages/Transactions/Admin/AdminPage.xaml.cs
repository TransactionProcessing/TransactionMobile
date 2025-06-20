using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Admin;

namespace TransactionProcessor.Mobile.Pages.Transactions.Admin;

public partial class AdminPage : ContentPage
{
    private AdminPageViewModel viewModel => this.BindingContext as AdminPageViewModel;

    public AdminPage(AdminPageViewModel vm)
    {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}