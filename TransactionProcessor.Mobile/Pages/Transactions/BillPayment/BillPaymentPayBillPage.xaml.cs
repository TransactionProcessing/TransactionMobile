using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentPayBillPage : ContentPage
{
    private BillPaymentPayBillPageViewModel viewModel => this.BindingContext as BillPaymentPayBillPageViewModel;

	public BillPaymentPayBillPage(BillPaymentPayBillPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
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
        await this.viewModel.Initialise(CancellationToken.None);
    }

    
}