using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.Voucher;

public partial class VoucherIssueFailedPage : ContentPage
{
    private VoucherIssueFailedPageViewModel viewModel => this.BindingContext as VoucherIssueFailedPageViewModel;

	public VoucherIssueFailedPage(VoucherIssueFailedPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
}