namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using TransactionMobile.Maui.UiTests.Common;
using UITests;

public class TransactionsSelectMobileTopupOperatorPage : BasePage
{
    public TransactionsSelectMobileTopupOperatorPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "Select an Operator";

    #endregion
}