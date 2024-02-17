namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using Common;
using OpenQA.Selenium;
using UITests;

public class TransactionsMobileTopupSelectProductPage : BasePage2 {
    public TransactionsMobileTopupSelectProductPage(TestingContext testingContext) : base(testingContext) {

    }

    #region Properties

    protected override String Trait => "Select a Product";

    #endregion

    public async Task ClickProductButton(String productText) {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}