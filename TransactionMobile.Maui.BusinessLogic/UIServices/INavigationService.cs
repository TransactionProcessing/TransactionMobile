namespace TransactionMobile.Maui.UIServices;

using BusinessLogic.ViewModels.Transactions;

public interface INavigationService
{
    #region Methods

    Task GoBack();

    Task GoToHome();

    Task GoToMobileTopupFailedPage();

    Task GoToMobileTopupPerformTopupPage(ProductDetails productDetails,
                                             Decimal topupAmount);

    Task GoToMobileTopupSelectOperatorPage();

    Task GoToBillPaymentSelectOperatorPage();

    Task GoToBillPaymentSelectProductPage(ProductDetails productDetails);

    Task GoToAdminPage();

    Task GoToMobileTopupSelectProductPage(ProductDetails productDetails);

    Task GoToMobileTopupSuccessPage();

    Task GoToVoucherIssueSuccessPage();

    Task GoToVoucherIssueFailedPage();

    Task GoToBillPaymentSuccessPage();

    Task GoToBillPaymentFailedPage();

    Task PopToRoot();

    Task GoToVoucherSelectOperatorPage();

    Task GoToVoucherSelectProductPage(ProductDetails productDetails);

    Task GoToVoucherIssueVoucherPage(ProductDetails productDetails,
                                         Decimal voucherAmount);

    Task GoToBillPaymentGetAccountPage(ProductDetails productDetails);

    Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                    BillDetails billDetails);

    Task GoToLoginPage();

    Task GoToViewLogsPage();

    Task GoToMyAccountAddresses();
    Task GoToMyAccountContacts();
    Task GoToMyAccountDetails();

    #endregion
}