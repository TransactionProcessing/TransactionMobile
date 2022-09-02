namespace TransactionMobile.Maui.UIServices;

using BusinessLogic.ViewModels.MyAccount;
using Pages;
using Pages.MyAccount;
using Pages.Support;
using Pages.Transactions.Admin;
using Pages.Transactions.MobileTopup;
using Pages.Transactions.Voucher;

public class ShellNavigationService : INavigationService
{
    #region Methods

    public async Task GoBack() {
        await Shell.Current.Navigation.PopAsync();
    }

    public async Task GoToHome() {
        Application.Current.MainPage = new AppShell();
        await NavigateTo("///main/home");
    }

    public async Task GoToMobileTopupFailedPage() {
        await NavigateTo($"{nameof(MobileTopupFailedPage)}");
    }

    public async Task GoToMobileTopupPerformTopupPage(String operatorIdentifier,
                                                      Guid contractId,
                                                      Guid productId,
                                                      Decimal topupAmount) {
        await
            NavigateTo($"{nameof(MobileTopupPerformTopupPage)}?OperatorIdentifier={operatorIdentifier}&ContractId={contractId}&ProductId={productId}&TopupAmount={topupAmount}");
    }

    public async Task GoToMobileTopupSelectOperatorPage() {
        await NavigateTo($"{nameof(MobileTopupSelectOperatorPage)}");
    }

    public async Task GoToAdminPage() {
        await NavigateTo(nameof(AdminPage));
    }

    public async Task GoToMobileTopupSelectProductPage(String operatorIdentifier) {
        await NavigateTo($"{nameof(MobileTopupSelectProductPage)}?OperatorIdentifier={operatorIdentifier}");
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

    public async Task PopToRoot() {
        Shared.Logger.Logger.LogInformation($"navigating to root");
        await Shell.Current.Navigation.PopToRootAsync();
    }

    public async Task GoToVoucherSelectOperatorPage() {
        await NavigateTo(nameof(VoucherSelectOperatorPage));
    }

    public async Task GoToVoucherSelectProductPage(String operatorIdentifier) {
        await NavigateTo($"{nameof(VoucherSelectProductPage)}?OperatorIdentifier={operatorIdentifier}");
    }

    public async Task GoToVoucherIssueVoucherPage(String operatorIdentifier,
                                                  Guid contractId,
                                                  Guid productId,
                                                  Decimal voucherAmount) {
        await
            NavigateTo($"{nameof(VoucherPerformIssuePage)}?OperatorIdentifier={operatorIdentifier}&ContractId={contractId}&ProductId={productId}&VoucherAmount={voucherAmount}");
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

    private async Task NavigateTo(String route) {
        try {
            Shared.Logger.Logger.LogInformation($"navigating to {route}");
            await Shell.Current.GoToAsync(route);
            }
        catch(Exception e) {
            Exception ex = new Exception("Error navigating to {route}", e);
            Shared.Logger.Logger.LogError(ex);
        }
    }

    private async Task NavigateTo(ContentPage page){
        try
        {
            //Shared.Logger.Logger.LogInformation($"navigating to {route}");
            await Shell.Current.Navigation.PushAsync(page);
        }
        catch (Exception e)
        {
            //Exception ex = new Exception("Error navigating to {route}", e);
            Shared.Logger.Logger.LogError(e);
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