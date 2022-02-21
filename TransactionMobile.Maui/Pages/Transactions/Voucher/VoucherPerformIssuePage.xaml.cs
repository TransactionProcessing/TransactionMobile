namespace TransactionMobile.Maui;

using BusinessLogic.Models;
using BusinessLogic.ViewModels.Transactions;

public partial class VoucherPerformIssuePage : ContentPage
{
    private VoucherPerformIssuePageViewModel viewModel => BindingContext as VoucherPerformIssuePageViewModel;

	public VoucherPerformIssuePage(VoucherPerformIssuePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm.OnRecipientMobileNumberEntryCompleted = () =>
                                                  {
                                                      if (this.VoucherAmountEntry.IsEnabled)
                                                      {
                                                          this.VoucherAmountEntry.Focus();
                                                      }
                                                      else
                                                      {
                                                          this.CustomerEmailAddressEntry.Focus();
                                                      }
                                                  };
        vm.OnRecipientEmailAddressEntryCompleted = () =>
                                                   {
                                                       if (this.VoucherAmountEntry.IsEnabled)
                                                       {
                                                           this.VoucherAmountEntry.Focus();
                                                       }
                                                       else
                                                       {
                                                           this.CustomerEmailAddressEntry.Focus();
                                                       }
                                                   };
        vm.OnVoucherAmountEntryCompleted = () =>
                                              {
                                                  this.CustomerEmailAddressEntry.Focus();
                                              };
        vm.OnCustomerEmailAddressEntryCompleted = () =>
                                         {
                                             this.IssueVoucherButton.Focus();
                                         };
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (viewModel.VoucherAmount > 0)
        {
            this.VoucherAmountEntry.IsEnabled = false;
        }
    }

    
}