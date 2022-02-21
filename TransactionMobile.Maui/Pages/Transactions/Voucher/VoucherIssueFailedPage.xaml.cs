namespace TransactionMobile.Maui;

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