using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;

public partial class MobileTopupPerformTopupPage : ContentPage
{
    private MobileTopupPerformTopupPageViewModel viewModel => this.BindingContext as MobileTopupPerformTopupPageViewModel;

	public MobileTopupPerformTopupPage(MobileTopupPerformTopupPageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
        vm.OnCustomerMobileNumberEntryCompleted = () =>
                                                  {
                                                      if (this.TopupAmountEntry.IsReadOnly == false)
                                                      {
                                                          this.TopupAmountEntry.Focus();
                                                      }
                                                      else
                                                      {
                                                          this.CustomerEmailAddressEntry.Focus();
                                                      }
                                                  };
        vm.OnTopupAmountEntryCompleted = () =>
                                         {
                                             this.CustomerEmailAddressEntry.Focus();
                                         };
        vm.OnCustomerEmailAddressEntryCompleted = () =>
                                         {
                                             this.PerformTopupButton.Focus();
                                         };
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        if (this.viewModel.TopupAmount > 0)
        {
            this.TopupAmountEntry.IsReadOnly = true;
        }
    }

    
}