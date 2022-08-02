using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.UITests;

namespace TransactionMobile.Maui.UiTests.Pages
{
    public class ProfilePage : BasePage
    {
        protected override String Trait => "My Account";

        private readonly String LogoutButton;

        public ProfilePage() {
            this.LogoutButton = "LogoutButton";
        }

        public async Task ClickLogoutButton()
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.LogoutButton);
            element.Click();
        }
    }
}
