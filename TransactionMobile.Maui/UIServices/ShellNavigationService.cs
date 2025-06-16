namespace TransactionMobile.Maui.UIServices;

using BusinessLogic.Logging;
using BusinessLogic.ViewModels;
using BusinessLogic.ViewModels.MyAccount;
using BusinessLogic.ViewModels.Transactions;
using Pages;
using Pages.MyAccount;
using Pages.Reports;
using Pages.Support;
using Pages.Transactions.Admin;
using Pages.Transactions.BillPayment;
using Pages.Transactions.MobileTopup;
using Pages.Transactions.Voucher;

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
        Application.Current.MainPage = new AppShell();
        await NavigateTo("///main/home");
    }

    public async Task GoToMobileTopupFailedPage() {
        await NavigateTo($"{nameof(MobileTopupFailedPage)}");
    }

    public async Task GoToMobileTopupPerformTopupPage(ProductDetails productDetails,
                                                          Decimal topupAmount) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {"TopupAmount", topupAmount},
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };

        await
            NavigateTo($"{nameof(MobileTopupPerformTopupPage)}", d);
    }

    public async Task GoToMobileTopupSelectOperatorPage() {
        await NavigateTo($"{nameof(MobileTopupSelectOperatorPage)}");
    }

    public async Task GoToBillPaymentSelectOperatorPage() {
        await NavigateTo($"{nameof(BillPaymentSelectOperatorPage)}");
    }

    public async Task GoToBillPaymentSelectProductPage(ProductDetails productDetails) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await NavigateTo($"{nameof(BillPaymentSelectProductPage)}",d);
    }

    public async Task GoToAdminPage() {
        await NavigateTo(nameof(AdminPage));
    }

    public async Task GoToMobileTopupSelectProductPage(ProductDetails productDetails) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await NavigateTo($"{nameof(MobileTopupSelectProductPage)}", d);
    }

    public async Task GoToMobileTopupSuccessPage() {
        await NavigateTo($"{nameof(MobileTopupSuccessPage)}");
    }

    public async Task GoToVoucherIssueSuccessPage() {
        await NavigateTo($"{nameof(VoucherIssueSuccessPage)}");
    }

    public async Task GoToVoucherIssueFailedPage() {
        await NavigateTo($"{nameof(VoucherIssueFailedPage)}");
    }

    public async Task GoToBillPaymentSuccessPage() {
        await NavigateTo($"{nameof(BillPaymentSuccessPage)}");
    }

    public async Task GoToBillPaymentFailedPage() {
        await NavigateTo($"{nameof(BillPaymentFailedPage)}");
    }

    public async Task PopToRoot() {
        Logger.LogInformation($"navigating to root");
        await Shell.Current.Navigation.PopToRootAsync();
    }

    public async Task GoToVoucherSelectOperatorPage() {
        await NavigateTo(nameof(VoucherSelectOperatorPage));
    }

    public async Task GoToVoucherSelectProductPage(ProductDetails productDetails) {

        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };

        await NavigateTo($"{nameof(VoucherSelectProductPage)}", d);
    }

    public async Task GoToVoucherIssueVoucherPage(ProductDetails productDetails,
                                                      Decimal voucherAmount) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {"VoucherAmount", voucherAmount},
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await
            NavigateTo($"{nameof(VoucherPerformIssuePage)}", d);
    }

    public async Task GoToBillPaymentGetAccountPage(ProductDetails productDetails) {

        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await
            NavigateTo($"{nameof(BillPaymentGetAccountPage)}", d);
    }

    public async Task GoToBillPaymentGetMeterPage(ProductDetails productDetails){
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                        };
        await
            NavigateTo($"{nameof(BillPaymentGetMeterPage)}", d);
    }

    public async Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                                 BillDetails billDetails) {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                            {nameof(BillDetails), billDetails}
                                                                        };

        await
            NavigateTo($"{nameof(BillPaymentPayBillPage)}", d);
    }

    public async Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                                 MeterDetails meterDetails)
    {
        Dictionary<String, Object> d = new Dictionary<String, Object>() {
                                                                            {nameof(ProductDetails), productDetails},
                                                                            {nameof(MeterDetails), meterDetails}
                                                                        };

        await
            NavigateTo($"{nameof(BillPaymentPayBillPage)}", d);
    }

    public async Task GoToLoginPage() {
        Application.Current.MainPage = new AppShell();
        await NavigateTo("///loginpage");
    }

    public async Task GoToViewLogsPage() {
        await NavigateTo(nameof(ViewLogsPage));
    }

    public async Task GoToMyAccountAddresses() {
        MyAccountAddressesPage p = (MyAccountAddressesPage)MauiProgram.Container.Services.GetService(typeof(MyAccountAddressesPage));
        await NavigateTo(p);
    }

    public async Task GoToMyAccountContacts() {
        await NavigateTo(nameof(MyAccountContactPage));
    }

    public async Task GoToMyAccountDetails() {
        await NavigateTo(nameof(MyAccountDetailsPage));
    }

    public async Task GoToReportsSalesAnalysis(){
        await NavigateTo(nameof(ReportsSalesAnalysisPage));
    }

    public async Task GoToReportsBalanceAnalysis(){
        await NavigateTo(nameof(ReportsBalanceAnalysisPage));
    }

    private async Task NavigateTo(String route) {
        try {
            Logger.LogInformation($"navigating to {route}");
            await Shell.Current.GoToAsync(route);
            }
        catch(Exception e) {
            Logger.LogError("Error navigating to {route}", e);
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
            Logger.LogError("Error navigating to {route}", e);
        }
    }

    private async Task NavigateTo(ContentPage page){
        try
        {
            //Logger.LogInformation($"navigating to {route}");
            await Shell.Current.Navigation.PushAsync(page);
        }
        catch (Exception e)
        {
            Logger.LogError("Error navigating to {route}", e);
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