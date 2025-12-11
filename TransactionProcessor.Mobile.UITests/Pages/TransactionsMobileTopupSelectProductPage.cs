using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsMobileTopupSelectProductPage : BasePage2 {
    public TransactionsMobileTopupSelectProductPage(TestingContext testingContext) : base(testingContext) {

    }

    #region Properties

    protected override String Trait => "SelectaProduct";

    #endregion

    public async Task ClickProductButton(String productText) {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}