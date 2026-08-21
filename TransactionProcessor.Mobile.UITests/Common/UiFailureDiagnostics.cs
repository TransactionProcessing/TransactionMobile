using System.Text;
using System.Text.Json;
using OpenQA.Selenium;
using Reqnroll;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Common;

internal static class UiFailureDiagnostics
{
    public static async Task CaptureAsync(
        TestingContext testingContext,
        ScenarioContext scenarioContext,
        AppiumDriverWrapper appiumDriver,
        string trigger)
    {
        if (testingContext.FailureDiagnosticsCaptured)
        {
            return;
        }

        testingContext.FailureDiagnosticsCaptured = true;

        string scenarioName = SanitizePathPart(scenarioContext.ScenarioInfo.Title);
        string stamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff");
        string root = Path.Combine(Path.GetTempPath(), "TransactionProcessor.Mobile.UITests", "Failures", $"{scenarioName}-{stamp}");
        Directory.CreateDirectory(root);

        try
        {
            WriteSummary(root, testingContext, scenarioContext, appiumDriver, trigger);
            await WriteBackendTraceAsync(root, testingContext).ConfigureAwait(false);
            await WriteBackendHealthAsync(root, testingContext).ConfigureAwait(false);
            await WriteAppStateAsync(root).ConfigureAwait(false);
            WriteAppiumLogs(root, appiumDriver);
            WriteSeedSnapshot(root, testingContext);
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(root, "diagnostics-error.txt"), ex.ToString());
        }

        testingContext.Logger?.LogWarning($"UI failure diagnostics captured at {root}");
    }

    private static void WriteSummary(
        string root,
        TestingContext testingContext,
        ScenarioContext scenarioContext,
        AppiumDriverWrapper appiumDriver,
        string trigger)
    {
        var summary = new StringBuilder();
        summary.AppendLine($"Trigger: {trigger}");
        summary.AppendLine($"Scenario: {scenarioContext.ScenarioInfo.Title}");
        summary.AppendLine($"Platform: {AppiumDriverWrapper.MobileTestPlatform}");
        summary.AppendLine($"Appium log file: {appiumDriver.AppiumLogFilePath}");
        summary.AppendLine($"Appium service URL: {appiumDriver.ServiceUrl}");
        summary.AppendLine($"Config host: {testingContext.TestHostHelper?.ConfigHostUrlForCurrentPlatform() ?? string.Empty}");
        summary.AppendLine($"Scenario error: {scenarioContext.TestError}");

        File.WriteAllText(Path.Combine(root, "summary.txt"), summary.ToString());
    }

    private static async Task WriteBackendTraceAsync(string root, TestingContext testingContext)
    {
        if (testingContext.BackendHost == null)
        {
            File.WriteAllText(Path.Combine(root, "backend-trace.txt"), "Backend host was not available.");
            return;
        }

        try
        {
            string[] trace = testingContext.BackendHost.GetRequestTraceSnapshot();
            File.WriteAllLines(Path.Combine(root, "backend-trace.txt"), trace);
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(root, "backend-trace.txt"), ex.ToString());
        }
    }

    private static async Task WriteBackendHealthAsync(string root, TestingContext testingContext)
    {
        if (testingContext.BackendHost == null)
        {
            File.WriteAllText(Path.Combine(root, "backend-health.txt"), "Backend host was not available.");
            return;
        }

        try
        {
            using HttpClient client = testingContext.BackendHost.CreateClient();
            using HttpResponseMessage response = await client.GetAsync("/health").ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            File.WriteAllText(
                Path.Combine(root, "backend-health.txt"),
                $"Status: {(int)response.StatusCode} {response.StatusCode}{Environment.NewLine}{content}");
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(root, "backend-health.txt"), ex.ToString());
        }
    }

    private static async Task WriteAppStateAsync(string root)
    {
        if (AppiumDriverWrapper.Driver == null)
        {
            File.WriteAllText(Path.Combine(root, "app-state.txt"), "Appium driver was not available.");
            return;
        }

        try
        {
            string pageSource = AppiumDriverWrapper.Driver.PageSource;
            File.WriteAllText(Path.Combine(root, "page-source.xml"), pageSource);
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(root, "page-source-error.txt"), ex.ToString());
        }

        try
        {
            if (AppiumDriverWrapper.Driver is ITakesScreenshot screenshotDriver)
            {
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(Path.Combine(root, "screen.png"));
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(root, "screenshot-error.txt"), ex.ToString());
        }
    }

    private static void WriteAppiumLogs(string root, AppiumDriverWrapper appiumDriver)
    {
        File.WriteAllText(Path.Combine(root, "appium-startup-log.txt"), string.Join(Environment.NewLine, appiumDriver.GetStartupOutputSnapshot()));
        File.WriteAllText(Path.Combine(root, "appium-log-tail.txt"), appiumDriver.GetStartupLogTail());
    }

    private static void WriteSeedSnapshot(string root, TestingContext testingContext)
    {
        string seedJson = JsonSerializer.Serialize(testingContext.ScenarioSeed, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Path.Combine(root, "scenario-seed.json"), seedJson);
    }

    private static string SanitizePathPart(string value)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidChar, '_');
        }

        return value.Replace(' ', '_');
    }
}

