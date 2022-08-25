using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.UITests;

namespace TransactionMobile.Maui.UiTests.Pages
{
    using Drivers;
    using OpenQA.Selenium.Appium.Android;
    using Shouldly;
    using UITests.Common;

    public class ProfilePage : BasePage
    {
        protected override String Trait => "My Account";

        private readonly String LogoutButton;
        private readonly String AddressesButton;
        private readonly String ContactsButton;
        private readonly String AccountInfoButton;


        public ProfilePage() {
            this.LogoutButton = "LogoutButton";
            this.AddressesButton = "AddressesButton";
            this.ContactsButton = "ContactsButton";
            this.AccountInfoButton = "AccountInfoButton";
        }

        public async Task ClickLogoutButton()
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.LogoutButton);
            element.Click();
        }

        public async Task ClickAddressesButton()
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.AddressesButton);
            element.Click();

            var x = await AppiumDriverWrapper.Driver.GetPageSource();
        }

        public async Task ClickContactsButton()
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.ContactsButton);
            element.Click();
        }

        public async Task ClickAccountInfoButton()
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.AccountInfoButton);
            element.Click();
        }
    }

    public class ProfileAddressesPage : BasePage
    {
        public readonly String PrimaryAddressLabel;

        private readonly String AddressLine1Label;

        private readonly String AddressLine2Label;

        private readonly String AddressLine3Label;

        private readonly String AddressLine4Label;

        private readonly String AddressRegionLabel;

        private readonly String AddressTownLabel;

        private readonly String AddressPostalCodeLabel;

        protected override String Trait => "My Addresses";

        public ProfileAddressesPage() {
            this.PrimaryAddressLabel = "PrimaryAddressLabel";
            this.AddressLine1Label = "AddressLine1Label";
            this.AddressLine2Label = "AddressLine2Label";
            this.AddressLine3Label = "AddressLine3Label";
            this.AddressLine4Label = "AddressLine4Label";
            this.AddressRegionLabel = "AddressRegionLabel";
            this.AddressTownLabel = "AddressTownLabel";
            this.AddressPostalCodeLabel = "AddressPostCodeLabel";
        }

        public async Task IsPrimaryAddressShown() {
            await this.WaitForElementByAccessibilityId(this.PrimaryAddressLabel);
        }

        public async Task<String> GetAddressLineValue(Int32 lineNumber) {
            return lineNumber switch {
                1 => await this.GetLabelValue(this.AddressLine1Label),
                2 => await this.GetLabelValue(this.AddressLine2Label),
                3 => await this.GetLabelValue(this.AddressLine3Label),
                4 => await this.GetLabelValue(this.AddressLine4Label)
            };
        }
        public async Task<String> GetAddressRegionValue()
        {
            return await this.GetLabelValue(this.AddressRegionLabel);
        }
        public async Task<String> GetAddressTownValue()
        {
            return await this.GetLabelValue(this.AddressTownLabel);
        }
        public async Task<String> GetAddressPostalCodeValue()
        {
            return await this.GetLabelValue(this.AddressPostalCodeLabel);
        }
    }

    public class ProfileContactsPage : BasePage
    {
        public readonly String PrimaryContactLabel;

        private readonly String ContactNameLabel;

        private readonly String ContactEmailAddressLabel;

        private readonly String ContactMobileNumberLabel;

        protected override String Trait => "My Contacts";

        public ProfileContactsPage() {
            this.PrimaryContactLabel = "PrimaryContactLabel";
            this.ContactNameLabel = "ContactNameLabel";
            this.ContactEmailAddressLabel = "ContactEmailAddressLabel";
            this.ContactMobileNumberLabel = "ContactMobileNumberLabel";
        }

        public async Task IsPrimaryContactShown()
        {
            await this.WaitForElementByAccessibilityId(this.PrimaryContactLabel);
        }

        public async Task<String> GetContactNameValue()
        {
            return await this.GetLabelValue(this.ContactNameLabel);
        }
        public async Task<String> GetContactEmailAddressValue()
        {
            return await this.GetLabelValue(this.ContactEmailAddressLabel);
        }
        public async Task<String> GetContactMobileNumberValue()
        {
            return await this.GetLabelValue(this.ContactMobileNumberLabel);
        }
    }

    public class ProfileAccountInfoPage : BasePage
    {
        protected override String Trait => "My Details";

        public readonly String MerchantNameLabel;
        public readonly String BalanceLabel;
        public readonly String AvailableBalanceLabel;
        public readonly String LastStatementDateLabel;
        public readonly String NextStatementDateLabel;
        public readonly String SettlementScheduleLabel;

        public ProfileAccountInfoPage() {
            this.MerchantNameLabel = "MerchantNameLabel";
            this.BalanceLabel = "BalanceLabel";
            this.AvailableBalanceLabel = "AvailableBalanceLabel";
            this.LastStatementDateLabel = "LastStatementDateLabel";
            this.NextStatementDateLabel = "NextStatementDateLabel";
            this.SettlementScheduleLabel = "SettlementScheduleLabel";
        }

        public async Task<String> GetMerchantNameValue()
        {
            return await this.GetLabelValue(this.MerchantNameLabel);
        }

        public async Task<String> GetBalanceValue()
        {
            return await this.GetLabelValue(this.BalanceLabel);
        }

        public async Task<String> GetAvailableBalanceValue()
        {
            return await this.GetLabelValue(this.AvailableBalanceLabel);
        }

        public async Task<String> GetLastStatementDateValue()
        {
            return await this.GetLabelValue(this.LastStatementDateLabel);
        }

        public async Task<String> GetNextStatementDateValue()
        {
            return await this.GetLabelValue(this.NextStatementDateLabel);
        }

        public async Task<String> GetSettlementScheduleValue()
        {
            return await this.GetLabelValue(this.SettlementScheduleLabel);
        }
    }
}
