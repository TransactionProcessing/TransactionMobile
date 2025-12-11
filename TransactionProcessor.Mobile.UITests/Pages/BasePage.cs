using OpenQA.Selenium;
using Shared.IntegrationTesting;
using Shouldly;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages
{
    public abstract class BasePage2{
        protected readonly TestingContext TestingContext;

        protected abstract String Trait { get; }

        protected BasePage2(TestingContext testingContext)
        {
            this.TestingContext = testingContext;
        }

        public async Task AssertOnPage(TimeSpan? timeout = null){
            String message = $"Unable to verify on page: {this.GetType().Name} with trait {this.Trait} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";

            Should.NotThrow(async () => await this.WaitForElementByAccessibilityId(this.Trait, timeout), message);
        }

        //public async Task WaitForPageToLeave(TimeSpan? timeout = null)
        //{
        //    String message = "Unable to verify *not* on page: " + this.GetType().Name;
        //    Should.NotThrow(async () => await this.WaitForNoElementByAccessibilityId(this.Trait, timeout), message);
        //}

        public async Task<String> GetPageSource()
        {
            return await AppiumDriverWrapper.Driver.GetPageSource();
        }

        public async Task WaitForToastMessage(String toastMessage)
        {
            await AppiumDriverWrapper.Driver.WaitForToastMessage(AppiumDriverWrapper.MobileTestPlatform, toastMessage);
        }

        public void HideKeyboard()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
            {
                AppiumDriverWrapper.Driver.HideKeyboard();
            }
        }

        public async Task AcceptAlert()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                IWebElement acceptButton = await AppiumDriverWrapper.Driver.GetElement("PrimaryButton");
                acceptButton.Click();
            }
            else
            {
                IAlert a = await this.SwitchToAlert();
                a.Accept();
            }
        }

        public async Task DismissAlert()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                IWebElement acceptButton = await AppiumDriverWrapper.Driver.GetElement("SecondaryButton");
                acceptButton.Click();
            }
            else
            {
                IAlert a = await this.SwitchToAlert();
                a.Dismiss();
            }
        }

        public async Task<IAlert> SwitchToAlert()
        {
            IAlert alert = null;
            await Retry.For(async () => {
                                alert = AppiumDriverWrapper.Driver.SwitchTo().Alert();
                                alert.ShouldNotBeNull();
                            });
            return alert;
        }

        public void NavigateBack()
        {
            AppiumDriverWrapper.Driver.Navigate().Back();
        }

        public async Task<String> GetLabelValue(String labelAutomationId)
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(labelAutomationId);
            return element.Text;
        }

        internal async Task<IWebElement> WaitForElementByAccessibilityId(String accessibilityId, TimeSpan? timeout = null, Int32 i  = 0)
        {
            return await AppiumDriverWrapper.Driver.GetElement(accessibilityId);
        }

        //internal async Task WaitForNoElementByAccessibilityId(String accessibilityId, TimeSpan? timeout = null)
        //{
        //    await AppiumDriverWrapper.Driver.WaitForNoElementByAccessibilityId(accessibilityId, timeout);
        //}

    }
}
