using Microsoft.Maui.Controls;

namespace TransactionMobile.Maui.Pages.Transactions.Voucher;

using BusinessLogic.ViewModels.Transactions;

public partial class VoucherIssueFailedPage : ContentPage
{
    private VoucherIssueFailedPageViewModel viewModel => BindingContext as VoucherIssueFailedPageViewModel;

	public VoucherIssueFailedPage(VoucherIssueFailedPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}