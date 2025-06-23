using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Features;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[NonParallelizable]
public partial class HardwarePageNavigationFeature : BaseTestFixture
{
    #region Constructors

    public HardwarePageNavigationFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform) {
    }

    #endregion
}