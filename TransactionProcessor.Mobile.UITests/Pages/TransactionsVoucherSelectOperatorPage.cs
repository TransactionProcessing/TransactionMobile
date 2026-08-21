using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsVoucherSelectOperatorPage : BasePage2
{
    public TransactionsVoucherSelectOperatorPage(TestingContext testingContext) : base(testingContext)
    {
    }

    #region Properties
    protected override String Trait => "SelectanOperator";
    #endregion

    public async Task ClickOperatorButton(String operatorName)
    {
        await this.ClickByTitleFallbackAsync(operatorName, operatorName, "Unable to locate voucher operator tile.").ConfigureAwait(false);
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
