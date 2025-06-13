using Microsoft.Maui.Controls;

namespace TransactionMobile.Maui.Pages.Transactions.MobileTopup;

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