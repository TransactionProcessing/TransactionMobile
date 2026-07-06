using OpenQA.Selenium;
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
    


    public ReportsPage(TestingContext testingContext) : base(testingContext)
    {
        this.DailyPerformanceSummaryButton = "DailyPerformanceSummaryButton";
        this.TransactionMixButton = "TransactionMixButton";
    }

    public async Task ClickDailyPerformanceSummaryButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.DailyPerformanceSummaryButton);
        element.Click();
    }

    public async Task ClickTransactionMixButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.TransactionMixButton);
        element.Click();
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
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UploadLogsButton);
        element.Click();
    }

    public async Task ClickViewLogsButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.ViewLogsButton);
        element.Click();
    }
}
