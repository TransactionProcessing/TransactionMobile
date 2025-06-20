using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsMobileTopupSelectProductPage : BasePage2 {
    public TransactionsMobileTopupSelectProductPage(TestingContext testingContext) : base(testingContext) {

    }

    #region Properties

    protected override String Trait => "Select a Product";

    #endregion

    public async Task ClickProductButton(String productText) {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}