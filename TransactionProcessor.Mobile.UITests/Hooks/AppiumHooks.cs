using OpenQA.Selenium;
using Reqnroll;
using Shared.IntegrationTesting;
using Shared.Logger;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Hooks
{
    [Binding]
    public class AppiumHooks
    {
        private readonly AppiumDriverWrapper AppiumDriver;
        private readonly TestingContext TestingContext;
        private readonly ScenarioContext scenarioContext;

        public AppiumHooks(AppiumDriverWrapper appiumDriver,
                           TestingContext testingContext,
                           ScenarioContext scenarioContext) {
            this.AppiumDriver = appiumDriver;
            this.TestingContext = testingContext;
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 1)]
        public async Task StartApp()
        {
            if (this.TestingContext.Logger == null)
            {
                this.TestingContext.Logger = new NlogLogger();
            }

            await Retry.For(async () =>
            {
                try
                {
                    await this.AppiumDriver.StartAppAsync().ConfigureAwait(false);
                }
                catch
                {
                    await UiFailureDiagnostics.CaptureAsync(this.TestingContext, this.scenarioContext, this.AppiumDriver, "AppiumBeforeScenario").ConfigureAwait(false);
                    throw;
                }
            }).ConfigureAwait(false);
        }

        [AfterScenario(Order = 1)]
        public async Task ShutdownApp()
        {
            if (this.TestingContext.Logger == null)
            {
                this.TestingContext.Logger = new NlogLogger();
            }

            this.TestingContext.Logger.LogInformation("About to Shutdown App");

            try
            {
                foreach (LogEntry logEntry in this.AppiumDriver.GetLogs())
                {
                    this.TestingContext.Logger.LogInformation($"{logEntry.Timestamp}|{logEntry.Level}|{logEntry.Message}");
                }
            }
            catch (Exception ex)
            {
                this.TestingContext.Logger.LogWarning($"Unable to collect Appium logs during teardown: {ex.Message}");
            }
            finally
            {
                if (this.scenarioContext.TestError != null)
                {
                    await UiFailureDiagnostics.CaptureAsync(this.TestingContext, this.scenarioContext, this.AppiumDriver, "AfterScenario").ConfigureAwait(false);
                }

                await this.AppiumDriver.StopAppAsync().ConfigureAwait(false);
            }

            this.TestingContext.Logger.LogInformation("App Shutdown");
        }
    }
}
