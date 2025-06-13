using Microsoft.Maui.Controls;

namespace TransactionMobile.Maui.Pages.Transactions.Voucher;

using BusinessLogic.ViewModels.Transactions;

public partial class VoucherIssueSuccessPage : ContentPage
{
    private VoucherIssueSuccessPageViewModel viewModel => BindingContext as VoucherIssueSuccessPageViewModel;

	public VoucherIssueSuccessPage(VoucherIssueSuccessPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}