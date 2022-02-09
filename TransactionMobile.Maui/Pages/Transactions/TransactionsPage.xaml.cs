namespace TransactionMobile.Maui.Pages.Transactions;

using ViewModels.Transactions;

public partial class TransactionsPage : ContentPage
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