using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsVoucherSelectProductPage : BasePage2
{
    public TransactionsVoucherSelectProductPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform switch
    {
        MobileTestPlatform.iOS => "Select a Product",
        _ => "SelectaProduct"
    };

    #endregion

    public async Task ClickProductButton(String productText)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}