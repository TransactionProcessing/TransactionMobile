using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class ReportsPage : BasePage2
{
    protected override String Trait
    {
        get
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                return "Reports";
            }
            return "Reports";
        }
    }

    private readonly String DailyPerformanceSummaryButton;
    private readonly String TransactionMixButton;
    private readonly String RecentActivityAndReceiptReportButton;
    


    public ReportsPage(TestingContext testingContext) : base(testingContext)
    {
        this.DailyPerformanceSummaryButton = "DailyPerformanceSummaryButton";
        this.TransactionMixButton = "TransactionMixButton";
        this.RecentActivityAndReceiptReportButton = "RecentActivityAndReceiptReportButton";
    }

    public async Task ClickDailyPerformanceSummaryButton()
    {
        await this.ClickReportTileAsync(this.DailyPerformanceSummaryButton, "Daily Performance Summary").ConfigureAwait(false);
    }

    public async Task ClickTransactionMixButton()
    {
        await this.ClickReportTileAsync(this.TransactionMixButton, "Transaction Mix").ConfigureAwait(false);
    }

    public async Task ClickRecentActivityAndReceiptReportButton()
    {
        await this.ClickReportTileAsync(this.RecentActivityAndReceiptReportButton, "Recent Activity and Receipt Report").ConfigureAwait(false);
    }

    private async Task ClickReportTileAsync(String automationId, String title)
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
                    $"Unable to locate report tile. AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
    }

}

public class SupportPage : BasePage2{
    protected override String Trait{
        get{
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows){
                return "Support";
            }

            return "Support";
        }
    }

    private readonly String UploadLogsButton;
    private readonly String ViewLogsButton;

    public SupportPage(TestingContext testingContext) : base(testingContext)
    {
        this.UploadLogsButton = "UploadLogsButton";
        this.ViewLogsButton = "ViewLogsButton";
    }

    public async Task ClickUploadLogsButton()
    {
        await this.ClickSupportTileAsync(this.UploadLogsButton, "Upload Logs").ConfigureAwait(false);
    }

    public async Task ClickViewLogsButton()
    {
        await this.ClickSupportTileAsync(this.ViewLogsButton, "View Logs").ConfigureAwait(false);
    }

    private async Task ClickSupportTileAsync(String automationId, String title)
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
                    $"Unable to locate support tile. AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
    }
}
