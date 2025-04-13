using NUnit.Framework;
using TransactionMobile.Maui.UITests.Common;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Features;

//[TestFixture(MobileTestPlatform.Android, Category = "Android")]
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