namespace TransactionMobile.Maui.Pages.Transactions.BillPayment;

using BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentSuccessPage : ContentPage
{
    private BillPaymentSuccessPageViewModel viewModel => BindingContext as BillPaymentSuccessPageViewModel;

	public BillPaymentSuccessPage(BillPaymentSuccessPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}