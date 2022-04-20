using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Hooks
{
    [Binding]
    public class AppiumHooks
    {
        private readonly AppiumDriverWrapper _appiumDriver;

        public AppiumHooks(AppiumDriverWrapper appiumDriver)
        {
            _appiumDriver = appiumDriver;
            AppiumDriverWrapper.MobileTestPlatform = MobileTestPlatform.Android;
        }

        [BeforeScenario()]
        public void StartApp()
        {
            _appiumDriver.StartApp();
        }

        [AfterScenario()]
        public void ShutdownApp()
        {
            _appiumDriver.StopApp();
        }
    }
}
