using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Features;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
//[TestFixture(MobileTestPlatform.Windows, Category = "Windows")]
[NonParallelizable]
public partial class IOSEndToEndTestsFeature : BaseTestFixture
{
    #region Constructors

    public IOSEndToEndTestsFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform)
    {
    }

    #endregion
}