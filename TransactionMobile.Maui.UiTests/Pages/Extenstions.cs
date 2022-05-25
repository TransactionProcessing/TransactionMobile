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

        await Retry.For(async () =>
                        {
                            element = driver.FindElement(MobileBy.AccessibilityId(selector));
                            element.ShouldNotBeNull();
                        });
        return element;
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
                        });

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