using System;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Common;

public sealed record TestAppConfig
{
    public enum AppiumTraceMode
    {
        Summary,
        Verbose,
        Off,
    }

    public MobileTestPlatform Platform { get; init; }

    public string? AndroidAppPath { get; init; }

    public string? WindowsAppId { get; init; }

    public TimeSpan AppiumStartupTimeout { get; init; } = TimeSpan.FromSeconds(180);

    public AppiumTraceMode AppiumTrace { get; init; } = AppiumTraceMode.Summary;

    public static TestAppConfig Load(MobileTestPlatform defaultPlatform)
    {
        MobileTestPlatform platform = ResolvePlatform(defaultPlatform);
        string? androidAppPath = Environment.GetEnvironmentVariable("UITEST_ANDROID_APP_PATH");
        string? windowsAppId = Environment.GetEnvironmentVariable("UITEST_WINDOWS_APP_ID");
        TimeSpan appiumStartupTimeout = ResolveAppiumStartupTimeout();
        AppiumTraceMode appiumTrace = ResolveAppiumTraceMode();

        return platform switch
        {
            MobileTestPlatform.Android => new TestAppConfig
            {
                Platform = platform,
                AndroidAppPath = RequireValue(androidAppPath, "UITEST_ANDROID_APP_PATH"),
                AppiumStartupTimeout = appiumStartupTimeout,
                AppiumTrace = appiumTrace
            },
            MobileTestPlatform.Windows => new TestAppConfig
            {
                Platform = platform,
                WindowsAppId = RequireValue(windowsAppId, "UITEST_WINDOWS_APP_ID"),
                AppiumStartupTimeout = appiumStartupTimeout,
                AppiumTrace = appiumTrace
            },
            _ => throw new InvalidOperationException($"Unsupported platform '{platform}'."),
        };
    }

    private static AppiumTraceMode ResolveAppiumTraceMode()
    {
        string? traceModeValue = Environment.GetEnvironmentVariable("UITEST_APPIUM_TRACE_MODE");
        if (string.IsNullOrWhiteSpace(traceModeValue))
        {
            return AppiumTraceMode.Summary;
        }

        if (Enum.TryParse(traceModeValue, ignoreCase: true, out AppiumTraceMode parsed) == false)
        {
            throw new InvalidOperationException(
                $"Invalid UITEST_APPIUM_TRACE_MODE value '{traceModeValue}'. Expected Off, Summary, or Verbose.");
        }

        return parsed;
    }

    private static TimeSpan ResolveAppiumStartupTimeout()
    {
        string? timeoutValue = Environment.GetEnvironmentVariable("UITEST_APPIUM_STARTUP_TIMEOUT_SECONDS");
        if (string.IsNullOrWhiteSpace(timeoutValue))
        {
            return TimeSpan.FromSeconds(180);
        }

        if (int.TryParse(timeoutValue, out int timeoutSeconds) == false || timeoutSeconds <= 0)
        {
            throw new InvalidOperationException($"Invalid UITEST_APPIUM_STARTUP_TIMEOUT_SECONDS value '{timeoutValue}'. Expected a positive whole number of seconds.");
        }

        return TimeSpan.FromSeconds(timeoutSeconds);
    }

    private static MobileTestPlatform ResolvePlatform(MobileTestPlatform defaultPlatform)
    {
        string? platformValue = Environment.GetEnvironmentVariable("UITEST_PLATFORM");
        if (string.IsNullOrWhiteSpace(platformValue))
        {
            return defaultPlatform;
        }

        if (Enum.TryParse(platformValue, ignoreCase: true, out MobileTestPlatform parsed) == false)
        {
            throw new InvalidOperationException($"Invalid UITEST_PLATFORM value '{platformValue}'. Expected Android or Windows.");
        }

        return parsed;
    }

    private static string RequireValue(string? value, string variableName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required environment variable '{variableName}'.");
        }

        return value;
    }
}
