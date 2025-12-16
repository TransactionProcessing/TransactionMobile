using OpenQA.Selenium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsVoucherEnterVoucherIssueDetailsPage : BasePage2
{

    private readonly String RecipientMobileNumberEntry;
    private readonly String RecipientEmailAddressEntry;
    private readonly String VoucherAmountEntry;
    private readonly String CustomerEmailAddressEntry;
    private readonly String IssueVoucherButton;

    public TransactionsVoucherEnterVoucherIssueDetailsPage(TestingContext testingContext) : base(testingContext)
    {
        this.RecipientMobileNumberEntry = "RecipientMobileNumberEntry";
        this.RecipientEmailAddressEntry = "RecipientEmailAddressEntry";
        this.VoucherAmountEntry = "VoucherAmountEntry";
        this.CustomerEmailAddressEntry = "CustomerEmailAddressEntry";
        this.IssueVoucherButton = "IssueVoucherButton";
    }

    #region Properties

    protected override String Trait => "EnterVoucherIssueDetails";

    public async Task EnterRecipientMobileNumber(String recipientMobileNumber)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.RecipientMobileNumberEntry);

        element.SendKeys(recipientMobileNumber);
    }

    public async Task EnterRecipientEmailAddress(String recipientEmailAddress)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.RecipientEmailAddressEntry);

        element.SendKeys(recipientEmailAddress);
    }

    public async Task EnterVoucherAmount(String voucherAmount)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.VoucherAmountEntry);

        element.SendKeys(voucherAmount);
    }

    public async Task EnterCustomerEmailAddress(String customerEmailAddress)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

        element.SendKeys(customerEmailAddress);
    }

    public async Task ClickIssueVoucherButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.IssueVoucherButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}