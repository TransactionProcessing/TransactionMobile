using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class RecentActivityReportPage : BasePage2
{
    protected override string Trait => "RecentActivityReport";

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
        IWebElement element = await this.WaitForElementByAccessibilityId("RecentActivitySearchButton");
        element.Click();
    }

    public async Task ClickResult(string reference)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(reference);
        element.Click();
    }
}
