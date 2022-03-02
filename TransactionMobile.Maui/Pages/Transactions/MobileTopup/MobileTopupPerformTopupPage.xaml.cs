namespace TransactionMobile.Maui;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupPerformTopupPage : ContentPage
{
    private MobileTopupPerformTopupPageViewModel viewModel => BindingContext as MobileTopupPerformTopupPageViewModel;

	public MobileTopupPerformTopupPage(MobileTopupPerformTopupPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
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
        if (viewModel.TopupAmount > 0)
        {
            this.TopupAmountEntry.IsReadOnly = true;
        }
    }

    
}