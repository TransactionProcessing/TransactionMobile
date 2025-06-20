using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;

public partial class MobileTopupFailedPage : ContentPage
{
    private MobileTopupFailedPageViewModel viewModel => this.BindingContext as MobileTopupFailedPageViewModel;

	public MobileTopupFailedPage(MobileTopupFailedPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
}