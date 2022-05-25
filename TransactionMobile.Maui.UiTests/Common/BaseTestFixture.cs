using OpenQA.Selenium.Appium;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UITests.Common
{
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