using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentSuccessPage : ContentPage
{
    private BillPaymentSuccessPageViewModel viewModel => this.BindingContext as BillPaymentSuccessPageViewModel;

	public BillPaymentSuccessPage(BillPaymentSuccessPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
}