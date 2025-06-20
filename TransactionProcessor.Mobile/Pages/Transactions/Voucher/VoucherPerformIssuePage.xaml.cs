using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.Pages.Transactions.Voucher;

public partial class VoucherPerformIssuePage : ContentPage
{
    private VoucherPerformIssuePageViewModel viewModel => this.BindingContext as VoucherPerformIssuePageViewModel;

	public VoucherPerformIssuePage(VoucherPerformIssuePageViewModel vm)
	{
		this.InitializeComponent();
        this.BindingContext = vm;
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
        if (this.viewModel.VoucherAmount > 0)
        {
            this.VoucherAmountEntry.IsReadOnly = true;
        }
    }

    
}