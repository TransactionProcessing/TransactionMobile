namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using Common;
using Drivers;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
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

public class ViewLogsPage : BasePage2{
    public ViewLogsPage(TestingContext testingContext) : base(testingContext){
        
    }

    protected override String Trait
    {
        get
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                return "View Logs";
            }
            return "View Logs";
        }
    }



}