namespace TransactionMobile.Maui.UiTests.Features;

using Drivers;
using NUnit.Framework;
using UITests.Common;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
[NonParallelizable]
public partial class PageNavigationFeature : BaseTestFixture
{
    #region Constructors

    public PageNavigationFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform) {
    }

    #endregion
}