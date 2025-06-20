using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Features
{
    [TestFixture(MobileTestPlatform.Android, Category = "Android")]
    //[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
    [TestFixture(MobileTestPlatform.Windows, Category = "Windows")]
    [NonParallelizable]
    public partial class EndToEndTestsFeature : BaseTestFixture
    {
        #region Constructors

        public EndToEndTestsFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform)
        {
        }

        #endregion
    }
}
