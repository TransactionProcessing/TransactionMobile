namespace TransactionMobile.Maui;

using BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupSuccessPage : ContentPage
{
    private MobileTopupSuccessPageViewModel viewModel => BindingContext as MobileTopupSuccessPageViewModel;

	public MobileTopupSuccessPage(MobileTopupSuccessPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}