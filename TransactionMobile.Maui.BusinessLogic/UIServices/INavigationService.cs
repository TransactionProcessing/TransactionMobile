namespace TransactionMobile.Maui.UIServices;

public interface INavigationService
{
    #region Methods

    Task GoToHome();

    Task GoToMobileTopupFailedPage();

    Task GoToMobileTopupPerformTopupPage(String operatorIdentifier,
                                         Guid contractId,
                                         Guid productId,
                                         Decimal topupAmount);

    Task GoToMobileTopupSelectOperatorPage();

    Task GoToMobileTopupSelectProductPage(String operatorIdentifier);

    Task GoToMobileTopupSuccessPage();

    Task PopToRoot();

    #endregion
}