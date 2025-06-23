namespace TransactionMobile.Maui.Pages.Transactions.BillPayment;

using BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentFailedPage : ContentPage
{
    private BillPaymentFailedPageViewModel viewModel => BindingContext as BillPaymentFailedPageViewModel;

	public BillPaymentFailedPage(BillPaymentFailedPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}