namespace TransactionMobile.Maui.UITests.Drivers
{
    using System;
    using System.IO;
    using System.Reflection;
    using Common;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Appium.iOS;
    using OpenQA.Selenium.Appium.Mac;
    using OpenQA.Selenium.Appium.Service;
    using OpenQA.Selenium.Appium.Windows;

    public class AppiumDriver
    {
        #region Fields

        public static AndroidDriver AndroidDriver;

        public static IOSDriver iOSDriver;

        public static MacDriver MacDriver;

        public static MobileTestPlatform MobileTestPlatform;

        public static WindowsDriver WindowsDriver;

        #endregion

        #region Methods

        public void StartApp()
        {
            AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723).Build();

            if (appiumService.IsRunning == false)
            {
                appiumService.Start();
            }

            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                var driverOptions = new AppiumOptions();
                driverOptions.AddAdditionalCapability("adbExecTimeout", TimeSpan.FromMinutes(5).Milliseconds);
                driverOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, "Espresso");
                // TODO: Only do this locally
                driverOptions.AddAdditionalCapability(MobileCapabilityType.FullReset, true);
                driverOptions.AddAdditionalCapability("forceEspressoRebuild", true);
                driverOptions.AddAdditionalCapability("enforceAppInstall", true);
                driverOptions.AddAdditionalCapability("noSign", true);
                driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "emulator-5554");

                String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net6.0-android/");
                var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.pos-Signed.apk");
                driverOptions.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
                driverOptions.AddAdditionalCapability("espressoBuildConfig",
                                                      "{ \"additionalAppDependencies\": [ \"com.google.android.material:material:1.0.0\", \"androidx.lifecycle:lifecycle-extensions:2.1.0\" ] }");

                AppiumDriver.AndroidDriver = new AndroidDriver(appiumService, driverOptions, TimeSpan.FromMinutes(10));
            }

            //if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            //{
            //    var driverOptions = new AppiumOptions();
            //    driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            //    driverOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone 11");
            //    driverOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "14.4");

            //    String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //    String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.iOS/bin/iPhoneSimulator/Release");
            //    var apkPath = Path.Combine(binariesFolder, "TransactionMobile.iOS.app");
            //    driverOptions.AddAdditionalCapability(MobileCapabilityType.App, apkPath);
            //    driverOptions.AddAdditionalCapability(MobileCapabilityType.FullReset, true);
            //    driverOptions.AddAdditionalCapability(MobileCapabilityType.AutomationName, "XCUITest");
            //    driverOptions.AddAdditionalCapability("useNewWDA", true);
            //    driverOptions.AddAdditionalCapability("wdaLaunchTimeout", 999999999);
            //    driverOptions.AddAdditionalCapability("wdaConnectionTimeout", 999999999);
            //    driverOptions.AddAdditionalCapability("restart", true);

            //    AppiumDriver.iOSDriver = new IOSDriver<IOSElement>(appiumService, driverOptions, TimeSpan.FromMinutes(5));
            //}

            // TODO: Implement iOS Tests
            // TODO: Implement Windows UI Tests
            // TODO: Implement Mac UI Tests
        }

        public void StopApp()
        {
            if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.Android)
            {
                AppiumDriver.AndroidDriver.Quit();
            }
            //else if (AppiumDriver.MobileTestPlatform == MobileTestPlatform.iOS)
            //{
            //    AppiumDriver.iOSDriver.CloseApp();
            //    AppiumDriver.iOSDriver.Quit();
            //}

            // TODO: Implement iOS Tests
            // TODO: Implement Windows UI Tests
            // TODO: Implement Mac UI Tests
        }

        #endregion
    }
}