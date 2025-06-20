using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Features;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
[TestFixture(MobileTestPlatform.Windows, Category = "Windows")]
[NonParallelizable]
public partial class PageNavigationFeature : BaseTestFixture
{
    #region Constructors

    public PageNavigationFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform) {
    }

    #endregion
}