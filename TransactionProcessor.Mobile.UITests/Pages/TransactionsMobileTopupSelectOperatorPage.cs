using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsMobileTopupSelectOperatorPage : BasePage2
{
    public TransactionsMobileTopupSelectOperatorPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "Select an Operator";

    #endregion

    public async Task ClickOperatorButton(String operatorName) {
        IWebElement element = await this.WaitForElementByAccessibilityId(operatorName);
        element.Click();
    }
}