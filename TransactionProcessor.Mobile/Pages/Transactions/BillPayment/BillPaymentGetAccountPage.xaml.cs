using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.BillPayment;

public partial class BillPaymentGetAccountPage : ContentPage
{
    private BillPaymentGetAccountPageViewModel viewModel => this.BindingContext as BillPaymentGetAccountPageViewModel;

	public BillPaymentGetAccountPage(BillPaymentGetAccountPageViewModel vm)
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