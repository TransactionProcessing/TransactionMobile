namespace TransactionMobile.Maui.Pages.Transactions;

using BusinessLogic.ViewModels.Transactions;
using TransactionMobile.Maui.Pages.Common;

public partial class TransactionsPage : NoBackWithoutLogoutPage
{
    private TransactionsPageViewModel viewModel => BindingContext as TransactionsPageViewModel;

    public TransactionsPage(TransactionsPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}