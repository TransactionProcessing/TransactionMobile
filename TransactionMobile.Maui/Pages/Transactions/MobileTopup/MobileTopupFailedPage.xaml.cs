namespace TransactionMobile.Maui;

using BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupFailedPage : ContentPage
{
    private MobileTopupFailedPageViewModel viewModel => BindingContext as MobileTopupFailedPageViewModel;

	public MobileTopupFailedPage(MobileTopupFailedPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}