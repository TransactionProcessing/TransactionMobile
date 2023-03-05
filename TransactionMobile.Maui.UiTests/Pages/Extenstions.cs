using OpenQA.Selenium.Appium;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UITests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using Shared.IntegrationTesting;
using Shouldly;

public static class Extenstions
{
    // TODO: Mac & Windows Extensions
    // TODO: May need a platform switch
    //public static AndroidElement GetAlert(this AndroidDriver<AndroidElement> driver)
    //{
    //    return driver.FindElementByClassName("androidx.appcompat.widget.AppCompatTextView");
    //}
    
    public static async Task<IWebElement> WaitForElementByAccessibilityId(this AppiumDriver driver,
                                                               String selector,
                                                               TimeSpan? timeout = null) {
        IWebElement? element = null;
        timeout ??= TimeSpan.FromSeconds(60);
        await Retry.For(async () => {
                            for (int i = 0; i < 10; i++) {

                                String message = $"Unable to find element on page: {selector} {Environment.NewLine} Source: {AppiumDriverWrapper.Driver.PageSource}";
                                driver.ScrollDown();
                                element = driver.FindElement(MobileBy.AccessibilityId(selector));
                                element.ShouldNotBeNull(message);
                                // All good so exit the loop
                                break;
                            }
                        });
        return element;
    }
    
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

    /*
     * don't forget that it's "natural scroll" where 
     * fromY is the point where you press the and toY where you release it
     */
    public static void Scroll(this AppiumDriver driver, int fromX, int fromY, int toX, int toY)
    {
        TouchAction touchAction = new TouchAction(driver);
        touchAction.LongPress(fromX, fromY).MoveTo(toX, toY).Release().Perform();
    }

    public static async Task WaitForNoElementByAccessibilityId(this AppiumDriver driver,
                                                               String selector,
                                                               TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(60);

        await Retry.For(async () =>
                        {
                            IWebElement? element = driver.FindElement(MobileBy.AccessibilityId(selector));
                            element.ShouldBeNull();
                        }, timeout);

    }

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
        else if (platform == MobileTestPlatform.iOS)
        {
            Boolean isDisplayed = false;
            int count = 0;
            do
            {
                if (driver.PageSource.Contains(expectedToast))
                {
                    Console.WriteLine(driver.PageSource);
                    isDisplayed = true;
                    break;
                }

                Thread.Sleep(200); //Add your custom wait if exists
                count++;

            } while (count < 10);

            Console.WriteLine(driver.PageSource);
            isDisplayed.ShouldBeTrue();
        }
    }
    
    public static async Task<String> GetPageSource(this AppiumDriver driver)
    {
        return driver.PageSource;
    }
}