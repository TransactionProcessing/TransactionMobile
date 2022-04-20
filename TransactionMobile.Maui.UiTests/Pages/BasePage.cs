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
    using Shouldly;

    public abstract class BasePage
    {
        protected abstract String Trait { get; }

        public async Task AssertOnPage(TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(60);

            await Retry.For(async () =>
            {
                String message = "Unable to verify on page: " + this.GetType().Name;

                Should.NotThrow(() => this.WaitForElementByAccessibilityId(this.Trait,"Label"), message);
            },
                            TimeSpan.FromMinutes(1),
                            timeout).ConfigureAwait(false);

            using (StreamWriter sw = new StreamWriter("C:\\Temp\\PageSource.log"))
            {
                sw.WriteLine(AppiumDriverWrapper.Driver.PageSource);
            }
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

        public async Task<IWebElement> WaitForElementByAccessibilityId(String accessibilityId, String type,TimeSpan? timeout = null)
        {
            return await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId(accessibilityId,type, timeout);
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
            AppiumDriverWrapper.Driver.HideKeyboard();
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
