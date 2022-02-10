using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UITests.Hooks
{
    using Drivers;
    using TechTalk.SpecFlow;

    [Binding]
    public class AppiumHooks
    {
        private readonly AppiumDriver _appiumDriver;

        public AppiumHooks(AppiumDriver appiumDriver)
        {
            _appiumDriver = appiumDriver;
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
