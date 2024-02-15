namespace TransactionMobile.Maui.Pages.Transactions.BillPayment;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentGetMeterPage : ContentPage
{
    private BillPaymentGetMeterPageViewModel viewModel => BindingContext as BillPaymentGetMeterPageViewModel;

	public BillPaymentGetMeterPage(BillPaymentGetMeterPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }

    
}