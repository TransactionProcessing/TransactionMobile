using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class RecentActivityReportPage : BasePage2
{
    protected override string Trait => AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows ? "Recent Activity and Receipt Report" : "RecentActivityandReceiptReport";

    public RecentActivityReportPage(TestingContext testingContext) : base(testingContext)
    {
    }

    public async Task EnterSearchText(string searchText)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId("RecentActivitySearchText");
        element.Clear();
        element.SendKeys(searchText);
    }

    public async Task ClickSearchButton()
    {
        await this.ClickSearchButtonAsync("RecentActivitySearchButton", "Search").ConfigureAwait(false);
    }

    public async Task ClickResult(string reference)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(reference);
        element.Click();
    }

    private async Task ClickSearchButtonAsync(string automationId, string title)
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
                // Fall through to the Windows-specific lookup below.
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
                string pageSource = await this.GetPageSource().ConfigureAwait(false);
                throw new InvalidOperationException(
                    $"Unable to locate recent activity search button. AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
    }
}
