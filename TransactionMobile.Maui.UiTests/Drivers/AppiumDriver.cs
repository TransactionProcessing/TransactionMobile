using Google.Protobuf.WellKnownTypes;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UiTests.Drivers
{
    using Microsoft.Testing.Platform.Capabilities;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium.iOS;
    using OpenQA.Selenium.Appium.Service.Options;
    using OpenQA.Selenium.Appium.Windows;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Threading;
    
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
            var appPath = Path.Combine("/Users/runner/work/mobileapp", "TransactionMobile.Maui.app");

            Console.WriteLine($"Using app path: {appPath}");
            var x = Directory.GetFiles(appPath);
            //if (!Directory.Exists(appPath))
            //{
                

            //    throw new Exception($"App path does not exist: {appPath}");
            //}

            var caps = new AppiumOptions();
            caps.PlatformName = "iOS";
            caps.PlatformVersion = "17.2";
            caps.DeviceName = "iPhone 15";
            caps.AutomationName = "XCUITest";
            caps.App = appPath;
            caps.AddAdditionalAppiumOption("bundleId", "com.transactionprocessing.pos");
            caps.AddAdditionalAppiumOption("useNewWDA", true);
            caps.AddAdditionalAppiumOption("showXcodeLog", true);
            caps.AddAdditionalAppiumOption("wdaStartupRetries", 3);
            caps.AddAdditionalAppiumOption("wdaStartupRetryInterval", 10000);
            var udid = Environment.GetEnvironmentVariable("SIMULATOR_ID");
            caps.AddAdditionalAppiumOption("udid", udid);
            //AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver(appiumService, caps, TimeSpan.FromMinutes(10));
            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    //AppiumDriverWrapper.Driver = new IOSDriver(appiumService, caps, TimeSpan.FromMinutes(5));
                    var serverUri = new Uri("http://127.0.0.1:4723/");
                    AppiumDriverWrapper.Driver = new IOSDriver(serverUri, caps, TimeSpan.FromMinutes(10));
                    Console.WriteLine($"Driver laucnhed");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Driver init failed (attempt {attempt + 1}): {ex.Message}");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    if (attempt == 2) throw;
                }
            }
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
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.transactionprocessing.pos");
            driverOptions.AddAdditionalAppiumOption("enforceAppInstall", true);
            driverOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", "40000");
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);

            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net9.0-android/");

            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.pos-Signed.apk");
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
