using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UiTests.Pages
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Windows;
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
        public async Task LogoutMessageIsDisplayed(String logoutAlertTitle,
                                                   String logoutAlertMessage) {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows){
                
                IWebElement alert = await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId("ContentScrollViewer");

                var allLabels = alert.FindElements(MobileBy.ClassName("TextBlock"));
                allLabels[0].Text.ShouldBe(logoutAlertTitle);
                allLabels[1].Text.ShouldBe(logoutAlertMessage);
            }
            else{
                IAlert a = await this.SwitchToAlert();
            a.Text.ShouldBe($"{logoutAlertTitle}{Environment.NewLine}{logoutAlertMessage}");
            }
        }

        public async Task ClickBackButton() {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.BackButton);
            element.Click();
        }
    }
}
