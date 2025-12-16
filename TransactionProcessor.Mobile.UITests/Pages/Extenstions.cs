using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using Shared.IntegrationTesting;
using Shouldly;
using TransactionProcessor.Mobile.UITests.Drivers;
//using OpenQA.Selenium.Appium.MultiTouch;

namespace TransactionProcessor.Mobile.UITests.Pages;

public static class Extenstions
{
    /*
        // TODO: Mac & Windows Extensions
        // TODO: May need a platform switch
        //public static AndroidElement GetAlert(this AndroidDriver<AndroidElement> driver)
        //{
        //    return driver.FindElementByClassName("androidx.appcompat.widget.AppCompatTextView");
        //}

        public static async Task<IWebElement> WaitForElementByAccessibilityId(this AppiumDriver driver,
                                                                   String selector,
                                                                   TimeSpan? timeout = null,
                                                                   Int32 i = 0) {
            IWebElement? element = null;

            TimeSpan retryFor = timeout switch{
                null => TimeSpan.FromSeconds(60),
                _ => timeout.Value
            };

            //selector = $"com.transactionprocessor.mobile:id/{selector}";

            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows && i == 1){

                await Retry.For(async () => {
                                    for (int i = 0; i < 10; i++){
                                        String message = $"Unable to find element on page: {selector} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";
                                        var elements = AppiumDriverWrapper.Driver.FindElements(MobileBy.AccessibilityId("navViewItem"));
                                        elements.ShouldNotBeEmpty();
                                        element = elements.SingleOrDefault(e => e.Text == selector);
                                        element.ShouldNotBeNull();
                                    }
                                }, retryFor);
                return element;
            }

            await Retry.For(async () => {
                                for (int i = 0; i < 10; i++) {

                                    String message = $"Unable to find element on page: {selector} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";
                                    driver.ScrollDown();
                                    element = driver.FindElementX(selector, "com.transactionprocessor.mobile");
                                    element.ShouldNotBeNull(message);
                                    // All good so exit the loop
                                    break;
                                }
                            }, retryFor);
            return element;
        }
        */
    // TODO: make the GetElement method more generic to handle both Android and iOS and make the android package configurable
    //public static async Task<IWebElement> GetElement(this AppiumDriver driver,
    //                                                 string automationId,
    //                                                 string androidPackage = "com.transactionprocessor.mobile") {
    //    TimeSpan retryFor = TimeSpan.FromSeconds(60);

    //    IWebElement element = null;
    //    await Retry.For(async () => {

    //        string fullResourceId = $"{androidPackage}:id/{automationId}";

    //        try {
    //            element = driver.FindElement(MobileBy.Id(fullResourceId));
    //        }
    //        catch (NoSuchElementException) {
    //            // Ignore and try AccessibilityId
    //        }

    //        if (element == null) {
    //            try {
    //                element = driver.FindElement(MobileBy.AccessibilityId(automationId));
    //            }
    //            catch (NoSuchElementException) {

    //            }
    //        }

    //        element.ShouldNotBeNull($"element not found. used fullResourceId [{fullResourceId}] and automationId [{automationId}]");
    //    }, retryFor);
    //    return element;
    //}
    public static async Task<IWebElement> GetElement(this AppiumDriver driver,
                                                     string automationId,
                                                     string androidPackage = "com.transactionprocessor.mobile")
    {
        TimeSpan retryFor = TimeSpan.FromSeconds(60);

        IWebElement element = null;

        await Retry.For(async () =>
        {
            try
            {
                // Determine platform at runtime
                var platform = driver.Capabilities.GetCapability("platformName")?.ToString()?.ToLowerInvariant();

                if (platform == "android")
                {
                    string fullResourceId = $"{androidPackage}:id/{automationId}";
                    try
                    {
                        element = driver.FindElement(MobileBy.Id(fullResourceId));
                    }
                    catch (NoSuchElementException)
                    {
                        // fallback to AccessibilityId
                    }
                }

                if (element == null)
                {
                    try
                    {
                        element = driver.FindElement(MobileBy.AccessibilityId(automationId));
                    }
                    catch (NoSuchElementException)
                    {
                        // do nothing; handled by retry
                    }
                }

                element.ShouldNotBeNull($"element not found. used automationId [{automationId}]");
            }
            catch (WebDriverException ex)
            {
                // optionally log or handle intermittent failures
            }

        }, retryFor);

        return element;
    }

    /*
    public static void ScrollDown(this AppiumDriver driver)
    {
        //if pressX was zero it didn't work for me
        int pressX = driver.Manage().Window.Size.Width / 2;
        // 4/5 of the screen as the bottom finger-press point
        int bottomY = driver.Manage().Window.Size.Height * 4 / 5;
        // just non zero point, as it didn't scroll to zero normally
        int topY = driver.Manage().Window.Size.Height / 8;
        //scroll with TouchAction by itself
        if (driver is WindowsDriver){
            return;
        }
        else{
            driver.Scroll(pressX, bottomY, pressX, topY);
        }
    }

    public static void Scroll(this AppiumDriver driver, int fromX, int fromY, int toX, int toY)
    {
        // TODO: Implement this for new Appium Driver
        //TouchAction touchAction = new TouchAction(driver);
        //touchAction.LongPress(fromX, fromY).MoveTo(toX, toY).Release().Perform();
    }

    public static async Task WaitForNoElementByAccessibilityId(this AppiumDriver driver,
                                                               String selector,
                                                               TimeSpan? timeout = null)
    {
        TimeSpan retryFor = timeout switch
        {
            null => TimeSpan.FromSeconds(60),
            _ => timeout.Value
        };

        await Retry.For(async () =>
                        {
                            IWebElement? element = driver.FindElement(MobileBy.AccessibilityId(selector));
                            element.ShouldBeNull();
                        }, retryFor);

    }
    */
    public static async Task WaitForToastMessage(this AppiumDriver driver, MobileTestPlatform platform, String expectedToast)
    {
        if (platform == MobileTestPlatform.Android)
        {
            await Retry.For(async () =>
            {
                Dictionary<String, Object> args = new Dictionary<string, object>
                {
                    {"text", expectedToast},
                    {"isRegexp", false}
                };
                driver.ExecuteScript("mobile: isToastVisible", args);

            });
        }
    }
    public static async Task<String> GetPageSource(this AppiumDriver driver)
    {
        return driver.PageSource;
    }
}