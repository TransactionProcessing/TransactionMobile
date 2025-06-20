using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentFailedPage : ContentPage
{
    private BillPaymentFailedPageViewModel viewModel => this.BindingContext as BillPaymentFailedPageViewModel;

	public BillPaymentFailedPage(BillPaymentFailedPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
}