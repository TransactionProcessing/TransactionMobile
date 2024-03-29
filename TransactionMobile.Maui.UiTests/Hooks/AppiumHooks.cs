﻿using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TransactionMobile.Maui.UiTests.Common;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Hooks
{
    using OpenQA.Selenium;

    [Binding]
    public class AppiumHooks
    {
        private readonly AppiumDriverWrapper AppiumDriver;
        private readonly TestingContext TestingContext;
        public AppiumHooks(AppiumDriverWrapper appiumDriver,
                           TestingContext testingContext) {
            this.AppiumDriver = appiumDriver;
            this.TestingContext = testingContext;
        }

        [BeforeScenario(Order = 0)]
        public void StartApp()
        {
            //this.TestingContext.Logger.LogInformation("About to Start App");
            this.AppiumDriver.StartApp();
            //this.TestingContext.Logger.LogInformation("App Started");
        }

        [AfterScenario(Order = 1)]
        public void ShutdownApp()
        {
            this.TestingContext.Logger.LogInformation("About to Shutdown App");
            var logs = this.AppiumDriver.GetLogs();
            if (logs != null) {
                foreach (LogEntry logEntry in logs) {
                    this.TestingContext.Logger.LogInformation($"{logEntry.Timestamp}|{logEntry.Level}|{logEntry.Message}");
                }
            }
            this.AppiumDriver.StopApp();
            this.TestingContext.Logger.LogInformation("App Shutdown");
        }
    }
}
