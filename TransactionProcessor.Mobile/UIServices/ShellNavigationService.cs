using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;
using TransactionProcessor.Mobile.Pages.MyAccount;
using TransactionProcessor.Mobile.Pages.Reports;
using TransactionProcessor.Mobile.Pages.Support;
using TransactionProcessor.Mobile.Pages.Transactions.Admin;
using TransactionProcessor.Mobile.Pages.Transactions.BillPayment;
using TransactionProcessor.Mobile.Pages.Transactions.MobileTopup;
using TransactionProcessor.Mobile.Pages.Transactions.Voucher;

namespace TransactionProcessor.Mobile.UIServices;

public class ShellNavigationService : INavigationService
{
    private readonly INavigationParameterService NavigationParameterService;

    #region Methods

    public ShellNavigationService(INavigationParameterService navigationParameterService) {
        this.NavigationParameterService = navigationParameterService;
    }

    public async Task QuitApplication(){
        Application.Current.Quit();
    }

    public async Task GoBack() {
        try {
            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception e) {
            throw new Exception($"Error in GoBack {e.Message}");
        }
        
    }

    public async Task GoToHome() {
        await this.NavigateTo("///main/home");
    }

    public async Task GoToMobileTopupFailedPage() {
        await this.NavigateTo($"{nameof(MobileTopupFailedPage)}");
    }

    public async Task GoToMobileTopupPerformTopupPage(ProductDetails productDetails,
                                                          Decimal topupAmount) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {"TopupAmount", topupAmount},
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };

        await this.NavigateTo($"{nameof(MobileTopupPerformTopupPage)}", d);
    }

    public async Task GoToMobileTopupSelectOperatorPage() {
        await this.NavigateTo($"{nameof(MobileTopupSelectOperatorPage)}");
    }

    public async Task GoToBillPaymentSelectOperatorPage() {
        await this.NavigateTo($"{nameof(BillPaymentSelectOperatorPage)}");
    }

    public async Task GoToBillPaymentSelectProductPage(ProductDetails productDetails) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await this.NavigateTo($"{nameof(BillPaymentSelectProductPage)}",d);
    }

    public async Task GoToAdminPage() {
        await this.NavigateTo(nameof(AdminPage));
    }

    public async Task GoToMobileTopupSelectProductPage(ProductDetails productDetails) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await this.NavigateTo($"{nameof(MobileTopupSelectProductPage)}", d);
    }

    public async Task GoToMobileTopupSuccessPage() {
        await this.NavigateTo($"{nameof(MobileTopupSuccessPage)}");
    }

    public async Task GoToVoucherIssueSuccessPage() {
        await this.NavigateTo($"{nameof(VoucherIssueSuccessPage)}");
    }

    public async Task GoToVoucherIssueFailedPage() {
        await this.NavigateTo($"{nameof(VoucherIssueFailedPage)}");
    }

    public async Task GoToBillPaymentSuccessPage() {
        await this.NavigateTo($"{nameof(BillPaymentSuccessPage)}");
    }

    public async Task GoToBillPaymentFailedPage() {
        await this.NavigateTo($"{nameof(BillPaymentFailedPage)}");
    }

    public async Task PopToRoot() {
        Logger.LogInformation($"navigating to root");
        await Shell.Current.Navigation.PopToRootAsync();
    }

    public async Task GoToVoucherSelectOperatorPage() {
        await this.NavigateTo(nameof(VoucherSelectOperatorPage));
    }

    public async Task GoToVoucherSelectProductPage(ProductDetails productDetails) {

        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };

        await this.NavigateTo($"{nameof(VoucherSelectProductPage)}", d);
    }

    public async Task GoToVoucherIssueVoucherPage(ProductDetails productDetails,
                                                      Decimal voucherAmount) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {"VoucherAmount", voucherAmount},
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await this.NavigateTo($"{nameof(VoucherPerformIssuePage)}", d);
    }

    public async Task GoToBillPaymentGetAccountPage(ProductDetails productDetails) {

        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await this.NavigateTo($"{nameof(BillPaymentGetAccountPage)}", d);
    }

    public async Task GoToBillPaymentGetMeterPage(ProductDetails productDetails){
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await this.NavigateTo($"{nameof(BillPaymentGetMeterPage)}", d);
    }

    public async Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                                 BillDetails billDetails) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                            {nameof(BillDetails), billDetails}
                                                                        };

        await this.NavigateTo($"{nameof(BillPaymentPayBillPage)}", d);
    }

    public async Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                                 MeterDetails meterDetails)
    {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                            {nameof(MeterDetails), meterDetails}
                                                                        };

        await this.NavigateTo($"{nameof(BillPaymentPayBillPage)}", d);
    }

    public async Task GoToLoginPage() {
        Application.Current.MainPage = new AppShell();
        await this.NavigateTo("///login");
    }

    public async Task GoToViewLogsPage() {
        await this.NavigateTo(nameof(ViewLogsPage));
    }

    public async Task GoToMyAccountAddresses() {
        MyAccountAddressesPage p = (MyAccountAddressesPage)MauiProgram.Container.Services.GetService(typeof(MyAccountAddressesPage));
        await this.NavigateTo(p);
    }

    public async Task GoToMyAccountContacts() {
        await this.NavigateTo(nameof(MyAccountContactPage));
    }

    public async Task GoToMyAccountDetails() {
        await this.NavigateTo(nameof(MyAccountDetailsPage));
    }
    
    public async Task GoToDailyPerformanceSummaryPage()
    {
        await this.NavigateTo(nameof(DailyPerformanceSummaryPage));
    }

    public async Task GoToTransactionMixSummaryPage()
    {
        await this.NavigateTo(nameof(TransactionMixPage));
    }

    public async Task GoToTransactions() {
        await this.NavigateTo("///main/transactions");
    }

    private async Task NavigateTo(String route) {
        try {
            Logger.LogInformation($"navigating to {route}");
            await Shell.Current.GoToAsync(route);
            }
        catch(Exception e) {
            Logger.LogError($"Error navigating to {route}", e);
        }
    }

    private async Task NavigateTo(String route,IDictionary<String,Object> parameters)
    {
        try
        {
            this.NavigationParameterService.SetParameters(parameters);
            Logger.LogInformation($"navigating to {route}");
            await Shell.Current.GoToAsync(route);
        }
        catch (Exception e)
        {
            Logger.LogError($"Error navigating to {route} (String route,IDictionary<String,Object> parameters)", e);
        }
    }

    private async Task NavigateTo(ContentPage page){
        try
        {
            await Shell.Current.Navigation.PushAsync(page);
        }
        catch (Exception e)
        {
            Logger.LogError($"Error navigating to page (ContentPage page))", e);
        }
    }

    private void ClearNavigationStack() {
        List<Page> existingPages = Shell.Current.Navigation.NavigationStack.ToList();
        foreach (Page page in existingPages)
        {
            if (page != null)
            {
                Shell.Current.Navigation.RemovePage(page);
            }
        }
    }

    #endregion
}
