using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UiTests.Pages
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using Shouldly;
    using TransactionMobile.Maui.UiTests.Drivers;
    using UITests;

    public class SharedPage : BasePage
    {
        protected override String Trait => String.Empty;
        private readonly String BackButton;

        public SharedPage() {
            this.BackButton = "BackButton";
        }
        public void LogoutMessageIsDisplayed(String logoutAlertTitle,
                                             String logoutAlertMessage) {
            IAlert a = this.SwitchToAlert();
            a.Text.ShouldBe($"{logoutAlertTitle}{Environment.NewLine}{logoutAlertMessage}");
        }

        public async Task ClickBackButton() {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.BackButton);
            element.Click();
        }
    }
}
