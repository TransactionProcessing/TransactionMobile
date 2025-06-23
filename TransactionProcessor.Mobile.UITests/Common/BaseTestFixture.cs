using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Common
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