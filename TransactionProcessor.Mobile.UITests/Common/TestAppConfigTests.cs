using System;
using NUnit.Framework;
using Shouldly;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Common;

[TestFixture]
[NonParallelizable]
public class TestAppConfigTests
{
    private string? originalPlatform;
    private string? originalAndroidAppPath;
    private string? originalWindowsAppId;

    [SetUp]
    public void CaptureEnvironment()
    {
        this.originalPlatform = Environment.GetEnvironmentVariable("UITEST_PLATFORM");
        this.originalAndroidAppPath = Environment.GetEnvironmentVariable("UITEST_ANDROID_APP_PATH");
        this.originalWindowsAppId = Environment.GetEnvironmentVariable("UITEST_WINDOWS_APP_ID");
    }

    [TearDown]
    public void RestoreEnvironment()
    {
        Environment.SetEnvironmentVariable("UITEST_PLATFORM", this.originalPlatform);
        Environment.SetEnvironmentVariable("UITEST_ANDROID_APP_PATH", this.originalAndroidAppPath);
        Environment.SetEnvironmentVariable("UITEST_WINDOWS_APP_ID", this.originalWindowsAppId);
    }

    [Test]
    public void Load_AndroidUsesAndroidAppPath()
    {
        Environment.SetEnvironmentVariable("UITEST_PLATFORM", "Android");
        Environment.SetEnvironmentVariable("UITEST_ANDROID_APP_PATH", @"C:\apps\TransactionProcessor.Mobile.apk");
        Environment.SetEnvironmentVariable("UITEST_WINDOWS_APP_ID", null);

        TestAppConfig config = TestAppConfig.Load(MobileTestPlatform.Windows);

        config.Platform.ShouldBe(MobileTestPlatform.Android);
        config.AndroidAppPath.ShouldBe(@"C:\apps\TransactionProcessor.Mobile.apk");
        config.WindowsAppId.ShouldBeNull();
    }

    [Test]
    public void Load_WindowsUsesWindowsAppId()
    {
        Environment.SetEnvironmentVariable("UITEST_PLATFORM", "Windows");
        Environment.SetEnvironmentVariable("UITEST_ANDROID_APP_PATH", null);
        Environment.SetEnvironmentVariable("UITEST_WINDOWS_APP_ID", "TransactionMobile_123!App");

        TestAppConfig config = TestAppConfig.Load(MobileTestPlatform.Android);

        config.Platform.ShouldBe(MobileTestPlatform.Windows);
        config.WindowsAppId.ShouldBe("TransactionMobile_123!App");
        config.AndroidAppPath.ShouldBeNull();
    }
}
