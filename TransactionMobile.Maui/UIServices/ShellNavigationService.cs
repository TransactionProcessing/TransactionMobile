namespace TransactionMobile.Maui.UIServices;

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

    public async Task GoToMobileTopupSelectProductPage(String operatorIdentifier)
    {
        await Shell.Current.GoToAsync($"{nameof(MobileTopupSelectProductPage)}?OperatorIdentifier={operatorIdentifier}");
    }

    public async Task GoToMobileTopupSuccessPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MobileTopupSuccessPage)}");
    }

    public async Task PopToRoot()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }

    #endregion
}