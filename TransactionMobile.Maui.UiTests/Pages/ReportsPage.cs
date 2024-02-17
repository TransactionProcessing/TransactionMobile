namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using Common;
using Drivers;
using OpenQA.Selenium;
using UITests;

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

    private readonly String SalesAnalysisButton;
    private readonly String BalanceAnalysisButton;
    


    public ReportsPage(TestingContext testingContext) : base(testingContext)
    {
        this.SalesAnalysisButton = "SalesAnalysisButton";
        this.BalanceAnalysisButton = "BalanceAnalysisButton";
    }
    public async Task ClickBalanceAnalysisButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.BalanceAnalysisButton);
        element.Click();
    }

    public async Task ClickSalesAnalysisButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.SalesAnalysisButton);
        element.Click();
    }
}