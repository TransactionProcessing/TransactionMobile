using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Net.Sockets;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace TransactionProcessor.Mobile.UITests.Drivers
{
    public enum MobileTestPlatform
    {
        Android,
        Windows,
    }

    public class AppiumDriverWrapper
    {
        private const int AppiumPort = 4723;
        private static readonly Uri AppiumServerUri = new($"http://127.0.0.1:{AppiumPort}/");

        public static MobileTestPlatform MobileTestPlatform;
        public static AppiumDriver Driver;

        public void StartApp()
        {
            bool appiumAlreadyRunning = this.IsPortOpen("127.0.0.1", AppiumPort);
            AppiumLocalService appiumService = null;

            if (appiumAlreadyRunning == false)
            {
                appiumService = new AppiumServiceBuilder().UsingPort(AppiumPort).Build();
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
            else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows)
            {
                AppiumDriverWrapper.SetupWindowsDriver(appiumService);
            }
        }

        private static void SetupWindowsDriver(AppiumLocalService? appiumService)
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
            AppiumDriverWrapper.Driver = appiumService == null
                ? new WindowsDriver(AppiumServerUri, driverOptions, TimeSpan.FromMinutes(10))
                : new WindowsDriver(appiumService, driverOptions, TimeSpan.FromMinutes(10));
        }
        
        private static void SetupAndroidDriver(AppiumLocalService? appiumService)
        {
            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalAppiumOption("adbExecTimeout", TimeSpan.FromMinutes(5).TotalMilliseconds);
            driverOptions.AutomationName = "UIAutomator2";
            driverOptions.PlatformName = "Android";
            driverOptions.PlatformVersion = "16.0";
            driverOptions.DeviceName = "emulator-5554";

            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.transactionprocessor.mobile");
            driverOptions.AddAdditionalAppiumOption("enforceAppInstall", true);
            driverOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", "40000");
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 6000);

            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionProcessor.Mobile/bin/Release/net10.0-android/");

            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessor.mobile-Signed.apk");
            var fileinfo = new FileInfo(apkPath);

            driverOptions.App = apkPath;

            AppiumDriverWrapper.Driver = appiumService == null
                ? new OpenQA.Selenium.Appium.Android.AndroidDriver(AppiumServerUri, driverOptions, TimeSpan.FromMinutes(5))
                : new OpenQA.Selenium.Appium.Android.AndroidDriver(appiumService, driverOptions, TimeSpan.FromMinutes(5));
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

        private bool IsPortOpen(string host, int port)
        {
            try
            {
                using TcpClient client = new TcpClient();
                IAsyncResult result = client.BeginConnect(host, port, null, null);
                bool completed = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                if (completed == false)
                {
                    client.Close();
                    return false;
                }

                client.EndConnect(result);
                return client.Connected;
            }
            catch
            {
                return false;
            }
        }
    }
}
