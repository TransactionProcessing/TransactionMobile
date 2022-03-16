namespace TransactionMobile.Maui.UIServices;

using Pages.Transactions.Admin;
using Pages.Transactions.MobileTopup;
using Pages.Transactions.Voucher;

public class ShellNavigationService : INavigationService
{
    #region Methods

    public async Task GoToHome()
    {
        await Shell.Current.GoToAsync("//home");
    }

    public async Task GoToMobileTopupFailedPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MobileTopupFailedPage)}");
    }

    public async Task GoToMobileTopupPerformTopupPage(String operatorIdentifier,
                                                      Guid contractId,
                                                      Guid productId,
                                                      Decimal topupAmount)
    {
        await
            Shell.Current.GoToAsync($"{nameof(MobileTopupPerformTopupPage)}?OperatorIdentifier={operatorIdentifier}&ContractId={contractId}&ProductId={productId}&TopupAmount={topupAmount}");
    }

    public async Task GoToMobileTopupSelectOperatorPage()
    {
        await Shell.Current.GoToAsync(nameof(MobileTopupSelectOperatorPage));
    }

    public async Task GoToAdminPage()
    {
        await Shell.Current.GoToAsync(nameof(AdminPage));
    }

    public async Task GoToMobileTopupSelectProductPage(String operatorIdentifier)
    {
        await Shell.Current.GoToAsync($"{nameof(MobileTopupSelectProductPage)}?OperatorIdentifier={operatorIdentifier}");
    }

    public async Task GoToMobileTopupSuccessPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MobileTopupSuccessPage)}");
    }

    public async Task GoToVoucherIssueSuccessPage()
    {
        await Shell.Current.GoToAsync($"{nameof(VoucherIssueSuccessPage)}");
    }

    public async Task GoToVoucherIssueFailedPage()
    {
        await Shell.Current.GoToAsync($"{nameof(VoucherIssueFailedPage)}");
    }

    public async Task PopToRoot()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }

    public async Task GoToVoucherSelectOperatorPage()
    {
        await Shell.Current.GoToAsync(nameof(VoucherSelectOperatorPage));
    }

    public async Task GoToVoucherSelectProductPage(String operatorIdentifier)
    {
        await Shell.Current.GoToAsync($"{nameof(VoucherSelectProductPage)}?OperatorIdentifier={operatorIdentifier}");
    }

    public async Task GoToVoucherIssueVoucherPage(String operatorIdentifier,
                                                  Guid contractId,
                                                  Guid productId,
                                                  Decimal voucherAmount)
    {
        await
            Shell.Current.GoToAsync($"{nameof(VoucherPerformIssuePage)}?OperatorIdentifier={operatorIdentifier}&ContractId={contractId}&ProductId={productId}&VoucherAmount={voucherAmount}");
    }

    #endregion
}