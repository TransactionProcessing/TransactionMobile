using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsPage : BasePage2
{
    protected override String Trait
    {
        get
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                return "SelectTransactionType";
            }

            return "Transactions";
        }
    }

    private readonly String MobileTopupButton;
    private readonly String VoucherButton;
    private readonly String BillPaymentButton;

    public TransactionsPage(TestingContext testingContext) : base(testingContext)
    {
        this.MobileTopupButton = "MobileTopupButton";
        this.VoucherButton = "VoucherButton";
        this.BillPaymentButton = "BillPaymentButton";
    }

    public async Task ClickMobileTopupButton()
    {
        await this.ClickTileAsync(this.MobileTopupButton, "Mobile Topup").ConfigureAwait(false);
    }

    public async Task ClickVoucherButton()
    {
        await this.ClickTileAsync(this.VoucherButton, "Voucher").ConfigureAwait(false);
    }

    public async Task ClickBillPaymentButton()
    {
        await this.ClickTileAsync(this.BillPaymentButton, "Bill Payment").ConfigureAwait(false);
    }

    private async Task ClickTileAsync(String automationId, String title)
    {
        await Retry.For(async () =>
        {
            IWebElement? element = null;

            try
            {
                element = await this.WaitForElementByAccessibilityId(automationId).ConfigureAwait(false);
            }
            catch
            {
                // Fall through to the Windows-specific lookup below.
            }

            if (element == null && AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                element = AppiumDriverWrapper.Driver.FindElements(MobileBy.Name(title)).FirstOrDefault();

                if (element == null)
                {
                    element = AppiumDriverWrapper.Driver.FindElements(MobileBy.XPath($"//*[contains(@Name,'{title}')]")).FirstOrDefault();
                }

                if (element == null)
                {
                    element = AppiumDriverWrapper.Driver.FindElements(MobileBy.XPath($"//*[@AutomationId='{automationId}']")).FirstOrDefault();
                }
            }

            if (element == null)
            {
                string pageSource = await this.GetPageSource().ConfigureAwait(false);
                throw new InvalidOperationException(
                    $"Unable to locate transactions option tile. AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
    }
}
