using OpenQA.Selenium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsVoucherIssueSuccessfulTopupPage : BasePage2
{

    private readonly String CompleteButton;

    public TransactionsVoucherIssueSuccessfulTopupPage(TestingContext testingContext) : base(testingContext)
    {
        this.CompleteButton = "CompleteButton";
    }

    #region Properties

    protected override String Trait => "VoucherIssueSuccessful";

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