using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace TransactionProcessor.Mobile.UITests.Drivers
{
    public enum MobileTestPlatform
    {
        iOS,
        Android,
        Windows,
        MacCatalyst
    }

    public class AppiumDriverWrapper
    {
        public static MobileTestPlatform MobileTestPlatform;
        public static AppiumDriver Driver;

        public void StartApp()
        {
            AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723).Build();

            if (appiumService.IsRunning == false)
            {
                appiumService.OutputDataReceived += (sender,
                                                     args) => {
                                                         Console.WriteLine(args.Data);
                                                         Debug.WriteLine(args.Data);
                                                     };
                appiumService.Start();
            }

            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
            {
                AppiumDriverWrapper.SetupAndroidDriver(appiumService);
            }
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS)
            {
                AppiumDriverWrapper.SetupiOSDriverNew(appiumService);
            }
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                AppiumDriverWrapper.SetupWindowsDriver(appiumService);
            }
        }

        private static void SetupWindowsDriver(AppiumLocalService appiumService)
        {
            var driverOptions = new AppiumOptions();
            driverOptions.AutomationName = "windows";
            driverOptions.PlatformName = "windows";
            driverOptions.DeviceName = "WindowsPC";

            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);
            driverOptions.AddAdditionalAppiumOption("ms:waitForAppLaunch", "50");
            //driverOptions.AddAdditionalAppiumOption("appium:createSessionTimeout", "100000");
            driverOptions.App = "TransactionMobile_zct748q4xfh0m!App";
            AppiumDriverWrapper.Driver = new WindowsDriver(appiumService, driverOptions, TimeSpan.FromMinutes(10));
        }

        public static void SetupiOSDriverX()
        {
            //String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionProcessor.Mobile/bin/Release/net9.0-ios/iossimulator-arm64/");
            //var appPath = Path.Combine(binariesFolder, "TransactionProcessor.Mobile.app");
            var appPath = "/Users/user272907/Documents/Projects/TransactionMobile/TransactionProcessor.Mobile/bin/Release/net9.0-ios/iossimulator-arm64/TransactionProcessor.Mobile.app";
            var exists = Directory.Exists(appPath);
            var options = new AppiumOptions();
            options.PlatformName = "iOS";
            options.PlatformVersion = "18.3";
            options.AutomationName = "XCUITest";
            options.DeviceName = "iPhone 16";
            options.App = appPath; // Only if you want Appium to install the app

            //var simulatorId = Environment.GetEnvironmentVariable("SIMULATOR_ID")?.Trim();
            //if (string.IsNullOrWhiteSpace(simulatorId))
            //    throw new InvalidOperationException("SIMULATOR_ID environment variable is not set.");
            //options.AddAdditionalAppiumOption("udid", simulatorId);

            //options.AddAdditionalAppiumOption("usePrebuiltWDA", true);
            //options.AddAdditionalAppiumOption("noReset", true);
            //options.AddAdditionalAppiumOption("fullReset", false);
            options.AddAdditionalAppiumOption("usePrebuiltWDA", true);
            options.AddAdditionalAppiumOption("derivedDataPath", "~/Library/Developer/Xcode/DerivedData/WebDriverAgent-hjlcwhatzxfnnggzdecgbewgjzil");
            options.AddAdditionalAppiumOption("wdaStartupRetries", 2);
            options.AddAdditionalAppiumOption("wdaStartupRetryInterval", 10000);
            options.AddAdditionalAppiumOption("showXcodeLog", true);
            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver(
                new Uri("http://127.0.0.1:4723"), options, TimeSpan.FromMinutes(2));
        }

        private static void SetupiOSDriverNew(AppiumLocalService appiumService)
        {
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionProcessor.Mobile/bin/Release/net9.0-ios/iossimulator-arm64/");
            var appPath = Path.Combine(binariesFolder, "TransactionProcessor.Mobile.app");

            var options = new AppiumOptions();
            options.PlatformName = "iOS";
            options.DeviceName = "";
            options.AddAdditionalAppiumOption("udid", "0A7AD110-C0C0-45BC-BCBC-8091AC55FF18");
            options.App = appPath;
            //options.AddAdditionalAppiumOption("bundleId", "com.apple.Preferences");
            options.AutomationName = "XCUITest";
            options.AddAdditionalAppiumOption("useNewWDA", true);
            options.AddAdditionalAppiumOption("autoAcceptAlerts", true);
            options.AddAdditionalAppiumOption("wdaStartupRetries", 3);
            options.AddAdditionalAppiumOption("wdaStartupRetryInterval", 5000);
            options.AddAdditionalAppiumOption("showXcodeLog", true);
            options.AddAdditionalAppiumOption("waitForIdleTimeout", 100);
            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver(appiumService, options);
        }

        private static void SetupiOSDriver(AppiumLocalService appiumService)
        {
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionProcessor.Mobile/bin/Release/net9.0-ios/iossimulator-arm64/");
            var appPath = Path.Combine(binariesFolder, "TransactionProcessor.Mobile.app");

            var options = new AppiumOptions();
            options.PlatformName = "iOS";
            options.PlatformVersion = "18.3";
            options.AutomationName = "XCUITest";
            options.DeviceName = "iPhone 16";
            options.App = appPath; // Only if you want Appium to install the app
                                   //options.AddAdditionalAppiumOption("useNewWDA", true); // Rebuild WDA
            options.AddAdditionalAppiumOption("usePrebuiltWDA", true);
            options.AddAdditionalAppiumOption("derivedDataPath", "~/Library/Developer/Xcode/DerivedData/WebDriverAgent-hjlcwhatzxfnnggzdecgbewgjziloption");
            options.AddAdditionalAppiumOption("wdaStartupRetries", 2);
            options.AddAdditionalAppiumOption("wdaStartupRetryInterval", 10000);

            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver(appiumService, options, TimeSpan.FromMinutes(2));
        }

        private static void SetupAndroidDriver(AppiumLocalService appiumService)
        {
            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalAppiumOption("adbExecTimeout", TimeSpan.FromMinutes(5).TotalMilliseconds);
            driverOptions.AutomationName = "UIAutomator2";
            driverOptions.PlatformName = "Android";
            driverOptions.PlatformVersion = "15.0";
            driverOptions.DeviceName = "emulator-5554";

            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.transactionprocessor.mobile");
            driverOptions.AddAdditionalAppiumOption("enforceAppInstall", true);
            driverOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", "40000");
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);

            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionProcessor.Mobile/bin/Release/net9.0-android/");

            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessor.mobile-Signed.apk");
            var fileinfo = new FileInfo(apkPath);

            driverOptions.App = apkPath;

            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.Android.AndroidDriver(appiumService, driverOptions, TimeSpan.FromMinutes(5));
        }

        public List<LogEntry> GetLogs()
        {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android)
            {
                if (AppiumDriverWrapper.Driver == null)
                {
                    return new List<LogEntry>();
                }
                ReadOnlyCollection<LogEntry>? logs = AppiumDriverWrapper.Driver.Manage().Logs.GetLog("logcat");
                return logs.ToList();
            }

            return null;
        }

        public void StopApp()
        {
            try
            {
                AppiumDriverWrapper.Driver?.Close();
                AppiumDriverWrapper.Driver?.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
