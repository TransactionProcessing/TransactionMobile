namespace TransactionMobile.Maui.UiTests.Features;

using Drivers;
using NUnit.Framework;
using UITests.Common;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[NonParallelizable]
public partial class HardwarePageNavigationFeature : BaseTestFixture
{
    #region Constructors

    public HardwarePageNavigationFeature(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform) {
    }

    #endregion
}