namespace TransactionMobile.Maui.UiTests.Pages
{
    using System;
    using System.Threading.Tasks;
    using Drivers;
    using OpenQA.Selenium;
    using UITests;

    public class ProfilePage : BasePage
    {
        #region Fields

        private readonly String AccountInfoButton;

        private readonly String AddressesButton;

        private readonly String ContactsButton;

        private readonly String LogoutButton;

        #endregion

        #region Constructors

        public ProfilePage() {
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

        public async Task ClickAccountInfoButton() {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.AccountInfoButton);
            element.Click();
        }

        public async Task ClickAddressesButton() {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.AddressesButton);
            element.Click();

            var x = await AppiumDriverWrapper.Driver.GetPageSource();
        }

        public async Task ClickContactsButton() {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.ContactsButton);
            element.Click();
        }

        public async Task ClickLogoutButton() {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.LogoutButton);
            element.Click();
        }

        #endregion
    }
}