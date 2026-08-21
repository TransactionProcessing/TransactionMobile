using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsVoucherSelectProductPage : BasePage2
{
    public TransactionsVoucherSelectProductPage(TestingContext testingContext) : base(testingContext)
    {
    }

    #region Properties

    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows ? "Select a Product" : "SelectaProduct";

    #endregion

    public async Task ClickProductButton(String productText)
    {
        String automationId = productText.Replace(" ", "");
        await this.ClickByTitleFallbackAsync(automationId, productText, "Unable to locate voucher product tile.").ConfigureAwait(false);
    }

    private async Task ClickByTitleFallbackAsync(String automationId, String title, String failureMessagePrefix)
    {
        await Retry.For(async () =>
        {
            IWebElement? element = null;

            try
            {
                element = await this.WaitForElementByAccessibilityId(automationId).ConfigureAwait(false);
            }
            catch
            {
            }

            if (element == null && AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                element = AppiumDriverWrapper.Driver.FindElements(MobileBy.Name(title)).FirstOrDefault();

                if (element == null)
                {
                    element = AppiumDriverWrapper.Driver.FindElements(MobileBy.XPath($"//*[contains(@Name,'{title}')]")).FirstOrDefault();
                }

                if (element == null)
                {
                    element = AppiumDriverWrapper.Driver.FindElements(MobileBy.XPath($"//*[@AutomationId='{automationId}']")).FirstOrDefault();
                }
            }

            if (element == null)
            {
                String pageSource = await this.GetPageSource().ConfigureAwait(false);
                throw new InvalidOperationException(
                    $"{failureMessagePrefix} AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
    }
}
