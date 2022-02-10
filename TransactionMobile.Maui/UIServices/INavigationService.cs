namespace TransactionMobile.Maui.UIServices;

public interface INavigationService
{
    Task GoToHome();

    Task GoToMobileTopupSuccessPage();

    Task GoToMobileTopupFailedPage();

    Task GoToMobileTopupSelectProductPage(String operatorIdentifier);

    Task GoToMobileTopupSelectOperatorPage();

    Task GoToMobileTopupPerformTopupPage(String operatorIdentifier,
                                         Guid contractId,
                                         Guid productId,
                                         Decimal topupAmount);

    Task PopToRoot();
}