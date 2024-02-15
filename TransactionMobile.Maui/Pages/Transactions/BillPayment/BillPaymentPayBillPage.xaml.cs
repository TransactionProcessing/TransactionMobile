namespace TransactionMobile.Maui.Pages.Transactions.BillPayment;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class BillPaymentPayBillPage : ContentPage
{
    private BillPaymentPayBillPageViewModel viewModel => BindingContext as BillPaymentPayBillPageViewModel;

	public BillPaymentPayBillPage(BillPaymentPayBillPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm.OnCustomerMobileNumberEntryCompleted = () =>
                                                  {
                                                      this.PostPaymentAmountEntry.Focus();
                                                  };
        vm.OnPaymentAmountEntryCompleted = () =>
                                                  {
                                                      this.MakePaymentButton.Focus();
                                                  };
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }

    
}