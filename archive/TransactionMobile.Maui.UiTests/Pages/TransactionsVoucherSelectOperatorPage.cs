namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using Common;
using OpenQA.Selenium;
using UITests;

public class TransactionsVoucherSelectOperatorPage : BasePage2
{
    public TransactionsVoucherSelectOperatorPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "Select an Operator";

    #endregion

    public async Task ClickOperatorButton(String operatorName)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(operatorName);
        element.Click();
    }
}