using TransactionMobile.Maui.UITests.Common;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Features;

using NUnit.Framework;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
public partial class LoginFeature : BaseTestFixture
{
    public LoginFeature(MobileTestPlatform mobileTestPlatform)
        : base(mobileTestPlatform)
    {
    }
}