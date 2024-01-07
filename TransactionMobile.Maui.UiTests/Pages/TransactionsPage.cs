namespace TransactionMobile.Maui.UiTests.Pages;

using OpenQA.Selenium;
using System;
using System.Threading.Tasks;
using Common;
using Drivers;
using UITests;

public class TransactionsPage : BasePage2
{
    private readonly TestingContext TestingContext;

    protected override String Trait{
        get{
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows){
                return "Select Transaction Type";
            }
            return "Transactions";
        }
    }

    private readonly String MobileTopupButton;
    private readonly String VoucherButton;


    public TransactionsPage(TestingContext testingContext) : base(testingContext)
    {
        this.MobileTopupButton = "MobileTopupButton";
        this.VoucherButton = "VoucherButton";
    }
    public async Task ClickMobileTopupButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.MobileTopupButton);
        element.Click();
    }

    public async Task ClickVoucherButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.VoucherButton);
        element.Click();
    }
}