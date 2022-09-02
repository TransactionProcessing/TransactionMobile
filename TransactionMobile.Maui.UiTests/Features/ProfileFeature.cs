namespace TransactionMobile.Maui.UiTests.Features;

using Drivers;
using NUnit.Framework;
using UITests.Common;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
[NonParallelizable]
public partial class ProfileFeature : BaseTestFixture
{
    #region Constructors

    public ProfileFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform) {
    }

    #endregion
}