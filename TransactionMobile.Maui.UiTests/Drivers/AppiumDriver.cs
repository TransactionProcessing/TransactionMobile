namespace TransactionMobile.Maui.UiTests.Drivers
{
    using System;
    using System.IO;
    using System.Reflection;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Appium.iOS;
    using OpenQA.Selenium.Appium.Service;

    public enum MobileTestPlatform
    {
        iOS,

        Android,

        Windows,

        MacCatalyst
    }

    public class AppiumDriverWrapper
    {
        #region Fields

        public static AppiumDriver Driver;

        public static MobileTestPlatform MobileTestPlatform;

        #endregion

        #region Methods

        public void StartApp() {
            var streamWriter = new StreamWriter("C:\\Temp\\Debugging.log", append:true);
            try {
                AppiumLocalService appiumService = new AppiumServiceBuilder().UsingPort(4723).Build();

                if (appiumService.IsRunning == false) {
                    appiumService.Start();
                    appiumService.OutputDataReceived += (sender,
                                                         args) => {
                                                            Console.WriteLine(args.Data);
                                                        };
                }

                if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android) {
                    AppiumDriverWrapper.SetupAndroidDriver(appiumService);
                }
                else if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS) {
                    AppiumDriverWrapper.SetupiOSDriver(appiumService);
                }
            }
            catch(Exception e) {
                streamWriter.Close();
                throw;
            }
        }

        public void StopApp() {
            AppiumDriverWrapper.Driver?.CloseApp();
        }

        private static void SetupAndroidDriver(AppiumLocalService appiumService) {
            // Do Android stuff to start up
            var driverOptions = new AppiumOptions();
            driverOptions.AddAdditionalAppiumOption("adbExecTimeout", TimeSpan.FromMinutes(5).Milliseconds);
            driverOptions.AutomationName = "UIAutomator2";
            driverOptions.PlatformName = "Android";
            driverOptions.PlatformVersion = "9.0";
            driverOptions.DeviceName = "emulator-5554";

            // TODO: Only do this locally
            //driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("appPackage", "com.transactionprocessing.pos");
            //driverOptions.AddAdditionalAppiumOption("forceEspressoRebuild", true);
            driverOptions.AddAdditionalAppiumOption("enforceAppInstall", true);
            //driverOptions.AddAdditionalAppiumOption("noSign", true);
            //driverOptions.AddAdditionalAppiumOption("espressoBuildConfig",
            //    "{ \"additionalAppDependencies\": [ \"com.google.android.material:material:1.0.0\", \"androidx.lifecycle:lifecycle-extensions:2.1.0\" ] }");

            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net6.0-android/");

            var apkPath = Path.Combine(binariesFolder, "com.transactionprocessing.pos.apk");
            driverOptions.App = apkPath;
            AppiumDriverWrapper.Driver = new AndroidDriver(appiumService, driverOptions, TimeSpan.FromMinutes(5));
        }

        private static void SetupiOSDriver(AppiumLocalService appiumService) {
            var driverOptions = new AppiumOptions();
            driverOptions.AutomationName = "XCUITest";
            driverOptions.PlatformName = "iOS";
            driverOptions.PlatformVersion = "15.4";
            driverOptions.DeviceName = "iPhone 11";

            String assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String binariesFolder = Path.Combine(assemblyFolder, "..", "..", "..", "..", @"TransactionMobile.Maui/bin/Release/net6.0-ios/iossimulator-x64/");
            var apkPath = Path.Combine(binariesFolder, "TransactionMobile.Maui.app");
            driverOptions.App = apkPath;
            driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.FullReset, true);
            driverOptions.AddAdditionalAppiumOption("useNewWDA", true);
            driverOptions.AddAdditionalAppiumOption("wdaLaunchTimeout", 999999999);
            driverOptions.AddAdditionalAppiumOption("wdaConnectionTimeout", 999999999);
            driverOptions.AddAdditionalAppiumOption("restart", true);
            driverOptions.AddAdditionalAppiumOption("simulatorStartupTimeout", 5 * 60 * 1000);

            AppiumDriverWrapper.Driver = new IOSDriver(appiumService, driverOptions, TimeSpan.FromMinutes(10));
        }

        #endregion
    }
}