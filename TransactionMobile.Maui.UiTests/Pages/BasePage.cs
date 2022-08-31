﻿using System;
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
                String message = $"Unable to verify on page: {this.GetType().Name} with trait {this.Trait} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";

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
            timeout = timeout ?? TimeSpan.FromSeconds(60);
            String message = "Unable to verify *not* on page: " + this.GetType().Name;

            Should.NotThrow(() => this.WaitForNoElementByAccessibilityId(this.Trait), message);
        }

        public async Task<IWebElement> WaitForElementByAccessibilityId(String accessibilityId, TimeSpan? timeout = null) {
            return await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId(accessibilityId, timeout);
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

        public void AcceptAlert()
        {
            IAlert a = this.SwitchToAlert();
            a.Accept();
        }

        public void DismissAlert()
        {
            IAlert a = this.SwitchToAlert();
            a.Dismiss();
        }

        public IAlert SwitchToAlert()
        {
            return AppiumDriverWrapper.Driver.SwitchTo().Alert();
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
    }
}
