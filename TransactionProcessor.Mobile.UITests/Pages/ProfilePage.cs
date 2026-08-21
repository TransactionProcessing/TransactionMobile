using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages
{
    public class ProfilePage : BasePage2
    {
        #region Fields

        private readonly String AccountInfoButton;

        private readonly String AddressesButton;

        private readonly String ContactsButton;

        private readonly String LogoutButton;

        #endregion

        #region Constructors

        public ProfilePage(TestingContext testingContext) : base(testingContext)
        {
            this.LogoutButton = "LogoutButton";
            this.AddressesButton = "AddressesButton";
            this.ContactsButton = "ContactsButton";
            this.AccountInfoButton = "AccountInfoButton";
        }

        #endregion

        #region Properties

        protected override String Trait => "My Account";

        #endregion

        #region Methods

        public async Task ClickAccountInfoButton()
        {
            await this.ClickOptionTileAsync(this.AccountInfoButton, "Account Info").ConfigureAwait(false);
        }

        public async Task ClickAddressesButton()
        {
            await this.ClickOptionTileAsync(this.AddressesButton, "Addresses").ConfigureAwait(false);
        }

        public async Task ClickContactsButton()
        {
            await this.ClickOptionTileAsync(this.ContactsButton, "Contacts").ConfigureAwait(false);
        }

        public async Task ClickLogoutButton()
        {
            await this.ClickOptionTileAsync(this.LogoutButton, "Logout").ConfigureAwait(false);
        }

        private async Task ClickOptionTileAsync(String automationId, String title)
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
                    // Fall back to Windows-specific lookup below.
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
                    String pageSource = await this.GetPageSource().ConfigureAwait(false);
                    throw new InvalidOperationException(
                        $"Unable to locate profile option tile. AutomationId: [{automationId}], Title: [{title}]{Environment.NewLine}Page source:{Environment.NewLine}{pageSource}");
                }

                element.Click();
            }).ConfigureAwait(false);
        }

        #endregion
    }
}
