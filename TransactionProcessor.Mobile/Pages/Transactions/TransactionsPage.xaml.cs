using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.Transactions;

public partial class TransactionsPage : NoBackWithoutLogoutPage
{
    private TransactionsPageViewModel viewModel => BindingContext as TransactionsPageViewModel;

    public TransactionsPage(TransactionsPageViewModel vm)
    {
        this.InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}