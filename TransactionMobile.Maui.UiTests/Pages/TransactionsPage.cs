namespace TransactionMobile.Maui.UiTests.Pages;

using OpenQA.Selenium;
using System;
using System.Threading.Tasks;
using UITests;

public class TransactionsPage : BasePage
{
    protected override String Trait => "Transactions";

    private readonly String MobileTopupButton;

    public TransactionsPage() {
        this.MobileTopupButton = "MobileTopupButton";


    }
    public async Task ClickMobileTopupButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.MobileTopupButton);
        element.Click();
    }
}

public class TransactionsSelectMobileTopupOperatorPage : BasePage
{
    protected override String Trait => "Select an Operator";
}