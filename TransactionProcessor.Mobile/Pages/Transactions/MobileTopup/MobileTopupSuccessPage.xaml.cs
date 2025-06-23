using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;

public partial class MobileTopupSuccessPage : ContentPage
{
    private MobileTopupSuccessPageViewModel viewModel => this.BindingContext as MobileTopupSuccessPageViewModel;

	public MobileTopupSuccessPage(MobileTopupSuccessPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
}