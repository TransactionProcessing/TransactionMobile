using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentGetMeterPage : ContentPage
{
    private BillPaymentGetMeterPageViewModel viewModel => this.BindingContext as BillPaymentGetMeterPageViewModel;

	public BillPaymentGetMeterPage(BillPaymentGetMeterPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }

    
}