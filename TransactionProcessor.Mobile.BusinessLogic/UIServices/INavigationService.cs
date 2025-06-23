using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.UIServices;

public interface INavigationService
{
    #region Methods

    Task QuitApplication();
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

    Task GoToBillPaymentGetMeterPage(ProductDetails productDetails);
    Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                    BillDetails billDetails);

    Task GoToBillPaymentPayBillPage(ProductDetails productDetails,
                                    MeterDetails meterDetails);

    Task GoToLoginPage();

    Task GoToViewLogsPage();

    Task GoToMyAccountAddresses();
    Task GoToMyAccountContacts();
    Task GoToMyAccountDetails();

    Task GoToReportsSalesAnalysis();
    Task GoToReportsBalanceAnalysis();

    #endregion
}