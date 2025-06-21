using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsVoucherSelectProductPage : BasePage2
{
    public TransactionsVoucherSelectProductPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "SelectaProduct";

    #endregion

    public async Task ClickProductButton(String productText)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}