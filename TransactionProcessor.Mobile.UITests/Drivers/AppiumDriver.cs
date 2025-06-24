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

        public void StartApp() {
            //OptionCollector o = new OptionCollector();
            //o.AddArguments(GeneralOptionList.BasePath("/wd/hub"));
            AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723)
                //.WithArguments(o)
                .Build();
            
            if (appiumService.IsRunning == false){
                appiumService.OutputDataReceived += (sender,
                                                     args) => {
                                                        //Console.WriteLine(args.Data);
                                                        Debug.WriteLine(args.Data);
                                                    };
                appiumService.Start();
            }

            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android){
                AppiumDriverWrapper.SetupAndroidDriver(appiumService);
            }
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS){
                AppiumDriverWrapper.SetupiOSDriver(appiumService);
            }
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                AppiumDriverWrapper.SetupWindowsDriver(appiumService);
            }
        }

        private static void SetupWindowsDriver(AppiumLocalService appiumService){
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

        private static void SetupiOSDriver(AppiumLocalService appiumService) {
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net8.0-ios/iossimulator-arm64/");
            var apkPath = Path.Combine(binariesFolder, "TransactionMobile.Maui.app");
            
            var caps = new AppiumOptions();
            caps.PlatformName = "iOS";
            caps.PlatformVersion = "18.0";
            caps.DeviceName = "iPhone 16";
            caps.AutomationName = "XCUITest";
            caps.App = apkPath;
            caps.AddAdditionalAppiumOption("fullReset", true);
            caps.AddAdditionalAppiumOption("noReset", false);
            caps.AddAdditionalAppiumOption("useNewWDA", true);

            //var driverOptions = new AppiumOptions();
            //driverOptions.AutomationName = "XCUITest";
            ////driverOptions.PlatformName = "iOS";
            ////driverOptions.PlatformVersion = "17.4";
            ////driverOptions.DeviceName = "iPhone 11";
            //driverOptions.AutomationName = "XCUITest";
            //driverOptions.PlatformName = "iOS";
            //driverOptions.PlatformVersion = "17.2";
            //driverOptions.AddAdditionalAppiumOption("udid", Environment.GetEnvironmentVariable("UDID")); // Corrected capability.

            //String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net8.0-ios/iossimulator-arm64/");
            //var apkPath = Path.Combine(binariesFolder, "TransactionMobile.Maui.app");
            //driverOptions.App = apkPath;
            //driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);
            //driverOptions.AddAdditionalAppiumOption("waitForQuiescence", false);
            //driverOptions.AddAdditionalAppiumOption("shouldWaitForQuiescence", false);
            //driverOptions.AddAdditionalAppiumOption("showXcodeLog", true);
            ////driverOptions.AddAdditionalAppiumOption("useNewWDA", true);
            ////driverOptions.AddAdditionalAppiumOption("wdaLaunchTimeout", 60000);
            ////driverOptions.AddAdditionalAppiumOption("wdaConnectionTimeout", 60000);
            ////driverOptions.AddAdditionalAppiumOption("wdaLaunchTimeout", 120000);
            ////driverOptions.AddAdditionalAppiumOption("wdaConnectionTimeout", 120000);
            ////driverOptions.AddAdditionalAppiumOption("useNewWDA", false);
            ////driverOptions.AddAdditionalAppiumOption("wdaStartupRetryInterval", 10000);
            ////driverOptions.AddAdditionalAppiumOption("wdaStartupRetries", 4);
            //driverOptions.AddAdditionalAppiumOption("usePrebuiltWDA", true);
            //driverOptions.AddAdditionalAppiumOption("skipServerInstallation", true);
            //driverOptions.AddAdditionalAppiumOption("skipProvisioningDeviceDetection", true);
            //driverOptions.AddAdditionalAppiumOption("wdaLocalPort", Environment.GetEnvironmentVariable("WDA_PATH"));
            //driverOptions.AddAdditionalAppiumOption("updatedWDABundleId", "WebDriverAgent/build");
            // Tell Appium to use the prebuilt WDA
            //driverOptions.AddAdditionalAppiumOption("usePrebuiltWDA", true);
            //driverOptions.AddAdditionalAppiumOption("useNewWDA", false);
            //driverOptions.AddAdditionalAppiumOption("shouldUseSingletonTestManager", true);

            //driverOptions.AddAdditionalAppiumOption("derivedDataPath", "/Users/runner/work/WebDriverAgent/build/Build/Products/Debug-iphonesimulator");
            //driverOptions.AddAdditionalAppiumOption("wdaLocalPort", 8100);


            //// Avoid unnecessary WDA rebuild attempts and add resilience
            //driverOptions.AddAdditionalAppiumOption("wdaLaunchTimeout", 60000);
            //driverOptions.AddAdditionalAppiumOption("wdaStartupRetries", 3);
            //driverOptions.AddAdditionalAppiumOption("wdaStartupRetryInterval", 10000);

            //// (Optional but often helpful)
            //driverOptions.AddAdditionalAppiumOption("waitForQuiescence", false);
            //driverOptions.AddAdditionalAppiumOption("startIWDP", true); // if you want to inspect webviews
            //driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, false);
            //driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, true);
            //driverOptions.AddAdditionalAppiumOption("usePrebuiltWDA", true);
            //driverOptions.AddAdditionalAppiumOption("wdaLocalPort", 8100); // optional, but helpful
            //driverOptions.AddAdditionalAppiumOption("shouldUseSingletonTestManager", true); // avoids extra processes
            //driverOptions.AddAdditionalAppiumOption("showXcodeLog", true); // shows build errors in logs
            //driverOptions.AddAdditionalAppiumOption("bundleId", "com.appium.WebDriverAgentRunner"); // match WDA bundle ID


            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver(appiumService, caps, TimeSpan.FromMinutes(10));
        }

        private static void SetupAndroidDriver(AppiumLocalService appiumService) {
            // Do Android stuff to start up
            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalAppiumOption("adbExecTimeout", TimeSpan.FromMinutes(5).TotalMilliseconds);
            driverOptions.AutomationName = "UIAutomator2";
            driverOptions.PlatformName = "Android";
            driverOptions.PlatformVersion = "15.0";
            driverOptions.DeviceName = "emulator-5554";
            
            // TODO: Only do this locally
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

        public List<LogEntry> GetLogs() {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android) {
                if (AppiumDriverWrapper.Driver == null) {
                    return new List<LogEntry>();
                }
                ReadOnlyCollection<LogEntry>? logs = AppiumDriverWrapper.Driver.Manage().Logs.GetLog("logcat");
                return logs.ToList();
            }

            return null;
        }

        public void StopApp()
        {
            try {
                AppiumDriverWrapper.Driver?.Close();
                AppiumDriverWrapper.Driver?.Quit();
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
