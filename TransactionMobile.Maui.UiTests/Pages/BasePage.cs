using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UITests
{
    using Common;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using Shouldly;

    public abstract class BasePage
    {
        protected abstract String Trait { get; }

        public async Task AssertOnPage(TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(60);

            await Retry.For(async () =>
            {
                String message = $"Unable to verify on page: {this.GetType().Name} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";

                Should.NotThrow(() => this.WaitForElementByAccessibilityId(this.Trait), message);
            },
                            TimeSpan.FromMinutes(1),
                            timeout).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the trait is no longer present. Defaults to a 5 second wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        public void WaitForPageToLeave(TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Should.NotThrow(() => this.WaitForNoElementByAccessibilityId(this.Trait), message);
        }

        public async Task<IWebElement> WaitForElementByAccessibilityId(String accessibilityId, TimeSpan? timeout = null)
        {
            IWebElement element = await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId(accessibilityId, timeout);
            TouchActions action = new TouchActions(AppiumDriverWrapper.Driver);
            action.Scroll(element, 0, 0);
            return element;
        }
        
        public async Task<String> GetPageSource()
        {
            return await AppiumDriverWrapper.Driver.GetPageSource();
        }

        public async Task WaitForNoElementByAccessibilityId(String accessibilityId)
        {
            await AppiumDriverWrapper.Driver.WaitForNoElementByAccessibilityId(accessibilityId);
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
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                AppiumDriverWrapper.Driver.FindElement(By.Name("Done")).Click();
            }
        }

        public IWebElement GetAlert()
        {
            return AppiumDriverWrapper.Driver.FindElement(By.Name("OK"));
        }

        public IAlert SwitchToAlert()
        {
            return AppiumDriverWrapper.Driver.SwitchTo().Alert();
        }

        public void NavigateBack()
        {
            AppiumDriverWrapper.Driver.Navigate().Back();
        }
    }
}
