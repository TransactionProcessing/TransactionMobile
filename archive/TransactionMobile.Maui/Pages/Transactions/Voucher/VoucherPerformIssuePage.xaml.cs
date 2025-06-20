namespace TransactionMobile.Maui.Pages.Transactions.Voucher;

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
                                                      if (this.VoucherAmountEntry.IsReadOnly == false)
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
        await this.viewModel.Initialise(CancellationToken.None);
        if (viewModel.VoucherAmount > 0)
        {
            this.VoucherAmountEntry.IsReadOnly = true;
        }
    }

    
}