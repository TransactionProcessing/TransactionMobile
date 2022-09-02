﻿namespace TransactionMobile.Maui.UIServices;

public interface INavigationService
{
    #region Methods

    Task GoBack();

    Task GoToHome();

    Task GoToMobileTopupFailedPage();

    Task GoToMobileTopupPerformTopupPage(String operatorIdentifier,
                                         Guid contractId,
                                         Guid productId,
                                         Decimal topupAmount);

    Task GoToMobileTopupSelectOperatorPage();

    Task GoToAdminPage();

    Task GoToMobileTopupSelectProductPage(String operatorIdentifier);

    Task GoToMobileTopupSuccessPage();

    Task GoToVoucherIssueSuccessPage();

    Task GoToVoucherIssueFailedPage();

    Task PopToRoot();

    Task GoToVoucherSelectOperatorPage();

    Task GoToVoucherSelectProductPage(String operatorIdentifier);

    Task GoToVoucherIssueVoucherPage(String operatorIdentifier,
                                         Guid contractId,
                                         Guid productId,
                                         Decimal voucherAmount);

    Task GoToLoginPage();

    Task GoToViewLogsPage();

    Task GoToMyAccountAddresses();
    Task GoToMyAccountContacts();
    Task GoToMyAccountDetails();

    #endregion
}