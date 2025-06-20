namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using Common;
using OpenQA.Selenium;
using Shared.IntegrationTesting;
using UITests;

public class TransactionsVoucherIssueSuccessfulTopupPage : BasePage2
{

    private readonly String CompleteButton;

    public TransactionsVoucherIssueSuccessfulTopupPage(TestingContext testingContext) : base(testingContext)
    {
        this.CompleteButton = "CompleteButton";
    }

    #region Properties

    protected override String Trait => "Voucher Issue Successful";

    public async Task ClickCompleteButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.CompleteButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}