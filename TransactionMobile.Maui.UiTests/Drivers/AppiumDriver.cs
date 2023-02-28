﻿using OpenQA.Selenium.Appium;
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
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium.Windows;

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

        public void StartApp(){
            AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723).Build();

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
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net7.0-windows10.0.19041.0/win10-x64/");
            var apkPath = Path.Combine(binariesFolder, "TransactionMobile.Maui.exe");
            Console.WriteLine(apkPath);

            Boolean folderExists = Directory.Exists(assemblyFolder);
            Console.Write(folderExists);
            if (folderExists) {
                var x = Directory.GetFiles(assemblyFolder);
                foreach (String s in x) {
                    Console.WriteLine(s);
                }
            }

            folderExists = Directory.Exists(binariesFolder);
            Console.Write(folderExists);
            if (folderExists){
                var x = Directory.GetFiles(binariesFolder);
                foreach (String s in x){
                    Console.WriteLine(s);
                }
            }
            
            driverOptions.App = apkPath;

            AppiumDriverWrapper.Driver = new WindowsDriver(appiumService, driverOptions, TimeSpan.FromMinutes(10));
        }

        private static void SetupiOSDriver(AppiumLocalService appiumService) {
            var driverOptions = new AppiumOptions();
            driverOptions.AutomationName = "XCUITest";
            driverOptions.PlatformName = "iOS";
            driverOptions.PlatformVersion = "15.4";
            driverOptions.DeviceName = "iPhone 11";
            
            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net7.0-ios/iossimulator-x64/");
            var apkPath = Path.Combine(binariesFolder, "TransactionMobile.Maui.app");
            driverOptions.App = apkPath;
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("useNewWDA", true);
            driverOptions.AddAdditionalAppiumOption("wdaLaunchTimeout", 999999999);
            driverOptions.AddAdditionalAppiumOption("wdaConnectionTimeout", 999999999);
            driverOptions.AddAdditionalAppiumOption("restart", true);
            driverOptions.AddAdditionalAppiumOption("simulatorStartupTimeout", 5 * 60 * 1000);
            
            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver(appiumService, driverOptions, TimeSpan.FromMinutes(10));
        }

        private static void SetupAndroidDriver(AppiumLocalService appiumService) {
            // Do Android stuff to start up
            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalAppiumOption("adbExecTimeout", TimeSpan.FromMinutes(5).TotalMilliseconds);
            driverOptions.AutomationName = "UIAutomator2";
            driverOptions.PlatformName = "Android";
            driverOptions.PlatformVersion = "9.0";
            driverOptions.DeviceName = "emulator-5554";
            
            // TODO: Only do this locally
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.transactionprocessing.pos");
            driverOptions.AddAdditionalAppiumOption("enforceAppInstall", true);
            driverOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", "40000");

            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net7.0-android/");

            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.pos-Signed.apk");
            driverOptions.App = apkPath;
            AppiumDriverWrapper.Driver = new OpenQA.Selenium.Appium.Android.AndroidDriver(appiumService, driverOptions, TimeSpan.FromMinutes(5));
            
        }

        public List<LogEntry> GetLogs() {
            if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android) {
                ReadOnlyCollection<LogEntry>? logs = AppiumDriverWrapper.Driver.Manage().Logs.GetLog("logcat");
                return logs.ToList();
            }

            return null;
        }

        public void StopApp()
        {
            try {
                AppiumDriverWrapper.Driver?.CloseApp();
                AppiumDriverWrapper.Driver?.Quit();
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
