namespace TransactionMobile.Maui.UITests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using Shouldly;
using UiTests.Common;
using UiTests.Drivers;

public class MainPage : BasePage2
{
    protected override String Trait => "Home";

    private readonly String TransactionsButton;

    private readonly String ReportsButton;

    private readonly String ProfileButton;

    private readonly String SupportButton;

    private readonly String AvailableBalanceLabel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage(TestingContext testingContext) : base(testingContext)
    {
        this.TransactionsButton = "Transactions";
        this.ReportsButton = "Reports";
        this.ProfileButton = "My Account";
        this.SupportButton = "SupportButton";
        this.AvailableBalanceLabel = "AvailableBalanceValueLabel";
    }

    public async Task ClickTransactionsButton(){
        var element = await this.WaitForElementByAccessibilityId(this.TransactionsButton, i:1);
        element.Click();
    }

    public async Task ClickReportsButton()
    {
        var element = await this.WaitForElementByAccessibilityId(this.ReportsButton, i:1);
        element.Click();
    }

    public async Task ClickProfileButton() {
        var element = await this.WaitForElementByAccessibilityId(this.ProfileButton,i:1);
        element.Click();
    }

    public async Task ClickSupportButton()
    {
        var element = await this.WaitForElementByAccessibilityId(this.SupportButton);
        element.Click();
    }

    public async Task<Decimal> GetAvailableBalanceValue(TimeSpan? timeout = default(TimeSpan?))
    {
        //await this.ScrollTo(this.Trait, this.AvailableBalanceLabel);
        //var element = await this.WaitForElementByAccessibilityId(this.AvailableBalanceLabel, timeout: TimeSpan.FromSeconds(30));

        //String availableBalanceText = element.Text.Replace(" KES", String.Empty);

        //if (Decimal.TryParse(availableBalanceText, out Decimal balanceValue) == false)
        //{
        //    throw new Exception($"Failed to parse [{availableBalanceText}] as a Decimal");
        //}

        //return balanceValue;
        return 0;
    }
}