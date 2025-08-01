﻿using OpenQA.Selenium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsMobileTopupSuccessfulTopupPage : BasePage2 {

    private readonly String CompleteButton;

    public TransactionsMobileTopupSuccessfulTopupPage(TestingContext testingContext) : base(testingContext) {
        this.CompleteButton = "CompleteButton";
    }

    #region Properties

    protected override String Trait => AppiumDriverWrapper.MobileTestPlatform switch
    {
        MobileTestPlatform.iOS => "Mobile Topup Successful",
        _ => "MobileTopupSuccessful"
    };

    public async Task ClickCompleteButton() {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.CompleteButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}