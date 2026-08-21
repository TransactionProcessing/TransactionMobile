using NLog;
using NUnit.Framework;
using Shared.Logger;
using Shouldly;
using TransactionProcessor.Mobile.UiTestBackend;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.SmokeTests;

[TestFixture(MobileTestPlatform.Android, Category = "Android")]
[TestFixture(MobileTestPlatform.Windows, Category = "Windows")]
[NonParallelizable]
public class LogoutLoginMerchantDetailsSmokeTests : BaseTestFixture
{
    private TestingContext testingContext = null!;
    private TestHostHelper testHostHelper = null!;
    private AppiumDriverWrapper appiumDriver = null!;
    private LoginPage loginPage = null!;
    private MainPage mainPage = null!;
    private ProfileAccountInfoPage profileAccountInfoPage = null!;
    private SharedPage sharedPage = null!;

    public LogoutLoginMerchantDetailsSmokeTests(MobileTestPlatform mobileTestPlatform) : base(mobileTestPlatform)
    {
    }

    [Test]
    public async Task LogoutThenRelogin_WithDifferentMerchant_ShowsFreshMerchantDetails()
    {
        await SetupHarnessAsync("LogoutThenReloginWithDifferentMerchant").ConfigureAwait(false);

        try
        {
            await this.loginPage.AssertOnPage().ConfigureAwait(false);
            await this.loginPage.SetConfigHostUrl(this.testHostHelper.ConfigHostUrlForCurrentPlatform()).ConfigureAwait(false);
            await this.loginPage.EnterEmailAddress("user1").ConfigureAwait(false);
            await this.loginPage.EnterPassword("123456").ConfigureAwait(false);
            await this.loginPage.ClickLoginButton().ConfigureAwait(false);

            await this.mainPage.AssertOnPage().ConfigureAwait(false);
            await this.mainPage.ClickProfileButton().ConfigureAwait(false);
            var profilePage = new ProfilePage(this.testingContext);
            await profilePage.AssertOnPage().ConfigureAwait(false);
            await profilePage.ClickAccountInfoButton().ConfigureAwait(false);
            await this.profileAccountInfoPage.AssertOnPage().ConfigureAwait(false);

            string merchantName = await this.profileAccountInfoPage.GetMerchantNameValue().ConfigureAwait(false);
            merchantName.ShouldBe("Test Merchant 1");

            await this.sharedPage.ClickBackButton().ConfigureAwait(false);
            await profilePage.AssertOnPage().ConfigureAwait(false);
            await this.sharedPage.ClickBackButton().ConfigureAwait(false);
            await this.mainPage.AssertOnPage().ConfigureAwait(false);
            await this.mainPage.ClickLogoutButton().ConfigureAwait(false);
            await this.sharedPage.LogoutMessageIsDisplayed("Title", "Logout Message").ConfigureAwait(false);
            await this.sharedPage.AcceptAlert().ConfigureAwait(false);
            await this.loginPage.AssertOnPage().ConfigureAwait(false);

            this.testHostHelper.ApplySeed(BuildMerchantSeed("Test Merchant 2", "user2"));

            await this.loginPage.EnterEmailAddress("user2").ConfigureAwait(false);
            await this.loginPage.EnterPassword("123456").ConfigureAwait(false);
            await this.loginPage.ClickLoginButton().ConfigureAwait(false);

            await this.mainPage.AssertOnPage().ConfigureAwait(false);
            await this.mainPage.ClickProfileButton().ConfigureAwait(false);
            var reloginProfilePage = new ProfilePage(this.testingContext);
            await reloginProfilePage.AssertOnPage().ConfigureAwait(false);
            await reloginProfilePage.ClickAccountInfoButton().ConfigureAwait(false);
            await this.profileAccountInfoPage.AssertOnPage().ConfigureAwait(false);

            string reloginMerchantName = await this.profileAccountInfoPage.GetMerchantNameValue().ConfigureAwait(false);
            reloginMerchantName.ShouldBe("Test Merchant 2");
        }
        finally
        {
            await this.CleanupHarnessAsync().ConfigureAwait(false);
        }
    }

    private async Task SetupHarnessAsync(string scenarioName)
    {
        await Setup.GlobalSetup().ConfigureAwait(false);

        NlogLogger logger = new();
        logger.Initialise(LogManager.GetLogger(scenarioName), scenarioName);
        LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);

        this.testingContext = new TestingContext();
        this.testingContext.Logger = logger;
        this.testHostHelper = new TestHostHelper(this.testingContext)
        {
            Logger = logger
        };
        this.testingContext.TestHostHelper = this.testHostHelper;

        this.appiumDriver = new AppiumDriverWrapper();

        await this.testHostHelper.StartTestHostForScenarioRun(scenarioName).ConfigureAwait(false);
        this.testHostHelper.ApplySeed(BuildMerchantSeed("Test Merchant 1", "user1"));
        await this.appiumDriver.StartAppAsync().ConfigureAwait(false);

        this.loginPage = new LoginPage(this.testingContext);
        this.mainPage = new MainPage(this.testingContext);
        this.profileAccountInfoPage = new ProfileAccountInfoPage(this.testingContext);
        this.sharedPage = new SharedPage(this.testingContext);
    }

    private async Task CleanupHarnessAsync()
    {
        if (this.appiumDriver != null)
        {
            await this.appiumDriver.StopAppAsync().ConfigureAwait(false);
        }

        if (this.testHostHelper != null)
        {
            await this.testHostHelper.StopTestHostForScenarioRun().ConfigureAwait(false);
        }
    }

    private static BackendSeed BuildMerchantSeed(string merchantName, string userName)
    {
        return new BackendSeed
        {
            Clients =
            [
                new ClientSeed
                {
                    ClientId = "mobileAppClient",
                    ClientName = "Mobile App Client",
                    Secret = "Secret1",
                    GrantTypes = ["password", "client_credentials"],
                    IsAppClient = true
                }
            ],
            Estates =
            [
                new EstateSeed
                {
                    EstateName = "Test Estate 1",
                    EstateReference = "Test Estate 1"
                }
            ],
            Merchants =
            [
                new MerchantSeed
                {
                    EstateName = "Test Estate 1",
                    MerchantName = merchantName,
                    AddressLine1 = "test address line 1",
                    AddressLine2 = "test address line 2",
                    AddressLine3 = "test address line 3",
                    AddressLine4 = "test address line 4",
                    Town = "TestTown",
                    Region = "Test Region",
                    PostalCode = "TE57 1NG",
                    Country = "United Kingdom",
                    ContactName = merchantName,
                    ContactEmailAddress = $"{merchantName.Replace(" ", string.Empty).ToLowerInvariant()}@merchant.example.com",
                    ContactPhoneNumber = "123456789"
                }
            ],
            Users =
            [
                new UserSeed
                {
                    UserName = userName,
                    Password = "123456",
                    GivenName = "TestMerchant",
                    FamilyName = userName
                }
            ]
        };
    }
}
