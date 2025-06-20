using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Shouldly;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages
{
    public class SharedPage : BasePage2
    {
        protected override String Trait => String.Empty;
        private readonly String BackButton;

        public SharedPage(TestingContext testingContext) : base(testingContext)
        {
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
