namespace TransactionMobile.Maui.UITests.Common
{
    using Drivers;

    public abstract class BaseTestFixture
    {
        #region Constructors

        protected BaseTestFixture(MobileTestPlatform mobileTestPlatform)
        {
            AppiumDriver.MobileTestPlatform = mobileTestPlatform;
        }

        #endregion
    }
}