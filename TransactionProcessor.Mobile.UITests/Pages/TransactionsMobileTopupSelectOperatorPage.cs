using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsMobileTopupSelectOperatorPage : BasePage2
{
    public TransactionsMobileTopupSelectOperatorPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties
    protected override String Trait => "SelectanOperator";

    #endregion

    public async Task ClickOperatorButton(String operatorName) {
        IWebElement element = await this.WaitForElementByAccessibilityId(operatorName);
        element.Click();
    }
}