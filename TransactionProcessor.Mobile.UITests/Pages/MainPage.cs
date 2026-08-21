using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

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
        this.SupportButton = "Support";
        this.AvailableBalanceLabel = "AvailableBalanceValueLabel";
    }

    public async Task ClickTransactionsButton()
    {
        await this.ClickTopLevelButtonAsync(this.TransactionsButton, "Transactions").ConfigureAwait(false);
    }

    public async Task ClickReportsButton()
    {
        await this.ClickTopLevelButtonAsync(this.ReportsButton, "Reports").ConfigureAwait(false);
    }

    public async Task ClickProfileButton()
    {
        await Retry.For(async () =>
        {
            IWebElement? element = null;

            try
            {
                element = await this.WaitForElementByAccessibilityId(this.ProfileButton, i: 1).ConfigureAwait(false);
            }
            catch
            {
                // Fall through to the Windows-specific lookup below.
            }

            if (element == null && AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                element = AppiumDriverWrapper.Driver.FindElements(MobileBy.Name(this.ProfileButton)).FirstOrDefault();

                if (element == null)
                {
                    element = AppiumDriverWrapper.Driver.FindElements(MobileBy.XPath($"//*[@Name='{this.ProfileButton}']")).FirstOrDefault();
                }
            }

            if (element == null)
            {
                string pageSource = await this.GetPageSource().ConfigureAwait(false);
                throw new InvalidOperationException(
                    $"Unable to locate the profile button using accessibility id or Windows fallbacks. AutomationId: [{this.ProfileButton}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
    }

    public async Task ClickSupportButton()
    {
        await this.ClickTopLevelButtonAsync(this.SupportButton, "Support").ConfigureAwait(false);
    }

    public async Task ClickLogoutButton()
    {
        await this.ClickTopLevelButtonAsync("BackButton", "Log Out").ConfigureAwait(false);
    }

    private async Task ClickTopLevelButtonAsync(String automationId, String title)
    {
        await Retry.For(async () =>
        {
            IWebElement? element = null;

            try
            {
                element = await this.WaitForElementByAccessibilityId(automationId, i: 1).ConfigureAwait(false);
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
                    $"Unable to locate top-level navigation button. AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
            }

            element.Click();
        }).ConfigureAwait(false);
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
