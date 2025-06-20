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
            String message = $"Unable to verify on page: {this.GetType().Name} with trait {this.Trait} {Environment.NewLine}";// Source: {AppiumDriverWrapper.Driver.PageSource}";

            Should.NotThrow(async () => await this.WaitForElementByAccessibilityId(this.Trait, timeout), message);
        }

        public async Task WaitForPageToLeave(TimeSpan? timeout = null)
        {
            String message = "Unable to verify *not* on page: " + this.GetType().Name;
            Should.NotThrow(async () => await this.WaitForNoElementByAccessibilityId(this.Trait, timeout), message);
        }

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
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                AppiumDriverWrapper.Driver.FindElement(By.Name("Done")).Click();
            }
        }

        public async Task AcceptAlert()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                IWebElement acceptButton = await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId("PrimaryButton");
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
                IWebElement acceptButton = await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId("SecondaryButton");
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
            return await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId(accessibilityId, timeout, i);
        }

        internal async Task WaitForNoElementByAccessibilityId(String accessibilityId, TimeSpan? timeout = null)
        {
            await AppiumDriverWrapper.Driver.WaitForNoElementByAccessibilityId(accessibilityId, timeout);
        }

    }

    //public abstract class BasePage
    //{
    //    protected readonly TestingContext TestingContext;

    //    protected abstract String Trait { get; }

    //    public BasePage(TestingContext testingContext){
    //        this.TestingContext = testingContext;
    //    }

    //    public async Task AssertOnPage(TimeSpan? timeout = null){
    //        var retryFor = timeout switch{
    //            null => TimeSpan.FromMinutes(3),
    //            _ => timeout.Value
    //        };

    //        await Retry.For(async () =>
    //        {
    //            String message = $"Unable to verify on page: {this.GetType().Name} with trait {this.Trait} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";

    //            Should.NotThrow(() => this.WaitForElementByAccessibilityId(this.Trait), message);
    //        },
    //                        retryFor,
    //                        TimeSpan.FromSeconds(60)).ConfigureAwait(false);
    //    }

    //    /// <summary>
    //    /// Verifies that the trait is no longer present. Defaults to a 5 second wait.
    //    /// </summary>
    //    /// <param name="timeout">Time to wait before the assertion fails</param>
    //    public async Task WaitForPageToLeave(TimeSpan? timeout = null)
    //    {
    //        var retryFor = timeout switch
    //        {
    //            null => TimeSpan.FromSeconds(60),
    //            _ => timeout.Value
    //        };

    //        String message = "Unable to verify *not* on page: " + this.GetType().Name;
    //        await Retry.For(async () => { Should.NotThrow(() => this.WaitForNoElementByAccessibilityId(this.Trait), message); }, retryFor);
    //    }

    //    public async Task<IWebElement> WaitForElementByAccessibilityId(String accessibilityId, TimeSpan? timeout = null, Int32 i = 0) {
    //        return await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId(accessibilityId, timeout,i);
    //    }
        
    //    public async Task<String> GetPageSource()
    //    {
    //        return await AppiumDriverWrapper.Driver.GetPageSource();
    //    }

    //    public async Task WaitForNoElementByAccessibilityId(String accessibilityId)
    //    {
    //        await AppiumDriverWrapper.Driver.WaitForNoElementByAccessibilityId(accessibilityId);
    //    }

    //    public async Task WaitForToastMessage(String toastMessage)
    //    {
    //        await AppiumDriverWrapper.Driver.WaitForToastMessage(AppiumDriverWrapper.MobileTestPlatform, toastMessage);
    //    }

    //    public void HideKeyboard()
    //    {
    //        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
    //        {
    //            AppiumDriverWrapper.Driver.HideKeyboard();
    //        }
    //        else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS)
    //        {
    //            AppiumDriverWrapper.Driver.FindElement(By.Name("Done")).Click();
    //        }
    //    }

    //    public async Task AcceptAlert(){
    //        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
    //        {
    //            IWebElement acceptButton = await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId("PrimaryButton");
    //            acceptButton.Click();
    //        }
    //        else{
    //            IAlert a = await this.SwitchToAlert();
    //            a.Accept();
    //        }
    //    }

    //    public async Task DismissAlert()
    //    {
    //        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
    //        {
    //            IWebElement acceptButton = await AppiumDriverWrapper.Driver.WaitForElementByAccessibilityId("SecondaryButton");
    //            acceptButton.Click();
    //        }
    //        else{
    //            IAlert a = await this.SwitchToAlert();
    //            a.Dismiss();
    //        }
    //    }

    //    public async Task<IAlert> SwitchToAlert(){
    //        IAlert alert = null;
    //        await Retry.For(async () => {
    //                            alert = AppiumDriverWrapper.Driver.SwitchTo().Alert();
    //                            alert.ShouldNotBeNull();
    //                        });
    //        return alert;
    //    }

    //    public void NavigateBack()
    //    {
    //        AppiumDriverWrapper.Driver.Navigate().Back();
    //    }

    //    public async Task<String> GetLabelValue(String labelAutomationId)
    //    {
    //        IWebElement element = await this.WaitForElementByAccessibilityId(labelAutomationId);
    //        return element.Text;
    //    }
    //}
}
