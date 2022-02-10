namespace TransactionMobile.Maui.UITests.Features;

using Common;
using NUnit.Framework;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
public partial class LoginFeature : BaseTestFixture
{
    public LoginFeature(MobileTestPlatform mobileTestPlatform)
        : base(mobileTestPlatform)
    {
    }
}