using TransactionMobile.Maui.UITests.Common;
using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UiTests.Features;

using NUnit.Framework;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.iOS, Category = "iOS")]
public partial class LoginFeature : BaseTestFixture
{
    public LoginFeature(MobileTestPlatform mobileTestPlatform)
        : base(mobileTestPlatform)
    {
    }
}