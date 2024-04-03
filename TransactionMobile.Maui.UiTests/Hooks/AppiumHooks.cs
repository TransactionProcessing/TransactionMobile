using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.UiTests.Common;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Hooks
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium.Enums;
    using Reqnroll;
    using Shared.IntegrationTesting;
    using Shouldly;

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
        public async Task StartApp()
        {
            //this.TestingContext.Logger.LogInformation("About to Start App");
            //this.TestingContext.Logger.LogInformation("App Started");

            await Retry.For(async () => {
                                this.AppiumDriver.StartApp();
                AppState state = AppiumDriverWrapper.Driver.GetAppState("com.transactionprocessing.pos");
                                state.ShouldBe(AppState.NotRunning);
                            });
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
