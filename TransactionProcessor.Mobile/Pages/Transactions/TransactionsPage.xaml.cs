using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.Transactions;

public partial class TransactionsPage : NoBackWithoutLogoutPage
{
    private TransactionsPageViewModel viewModel => BindingContext as TransactionsPageViewModel;

    public TransactionsPage(TransactionsPageViewModel vm,
                            INavigationService navigationService,
                            IDialogService dialogService,
                            IApplicationCache applicationCache)
        : base(navigationService, dialogService, applicationCache)
    {
        this.InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}
