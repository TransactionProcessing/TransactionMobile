using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.Voucher;

public partial class VoucherIssueSuccessPage : ContentPage
{
    private VoucherIssueSuccessPageViewModel viewModel => this.BindingContext as VoucherIssueSuccessPageViewModel;

	public VoucherIssueSuccessPage(VoucherIssueSuccessPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
}