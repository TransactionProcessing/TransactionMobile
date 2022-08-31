using NUnit.Framework;
using OpenQA.Selenium.Appium;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UITests.Common
{
    [NonParallelizable]
    public abstract class BaseTestFixture
    {
        #region Constructors

        protected BaseTestFixture(MobileTestPlatform mobileTestPlatform)
        {
            AppiumDriverWrapper.MobileTestPlatform = mobileTestPlatform;
        }

        #endregion
    }
}