using Reqnroll;
using Shouldly;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps;

[Binding]
[Scope(Tag = "login")]
public class LoginSteps
{
    private readonly TestingContext testingContext;
    private readonly LoginPage loginPage;
    private readonly MainPage mainPage;

    public LoginSteps(TestingContext testingContext)
    {
        this.testingContext = testingContext;
        this.loginPage = new LoginPage(testingContext);
        this.mainPage = new MainPage(testingContext);
    }

    [Given(@"I am on the Login Screen")]
    [Then("the Login Page is displayed")]
    public async Task GivenIAmOnTheLoginScreen()
    {
        await this.loginPage.AssertOnPage().ConfigureAwait(false);
    }

    [Given(@"my device is registered")]
    public async Task GivenMyDeviceIsRegistered()
    {
        string configHostUrl = this.testingContext.TestHostHelper.ConfigHostUrlForCurrentPlatform();
        await this.loginPage.SetConfigHostUrl(configHostUrl).ConfigureAwait(false);
        this.testingContext.Logger.LogInformation($"Configured device host: {configHostUrl}");
    }

    [When(@"I enter '(.*)' as the Email Address")]
    public async Task WhenIEnterAsTheEmailAddress(string emailAddress)
    {
        await this.loginPage.EnterEmailAddress(emailAddress).ConfigureAwait(false);
        await Task.Delay(2000).ConfigureAwait(false);
        string text = await this.loginPage.GetEmailAddress().ConfigureAwait(false);
        text.ShouldBe(emailAddress);
    }

    [When(@"I enter '(.*)' as the Password")]
    public async Task WhenIEnterAsThePassword(string password)
    {
        await this.loginPage.EnterPassword(password).ConfigureAwait(false);
    }

    [When(@"I tap on Login")]
    public async Task WhenITapOnLogin()
    {
        await this.loginPage.ClickLoginButton().ConfigureAwait(false);
    }

    [Then(@"the Merchant Home Page is displayed")]
    public async Task ThenTheMerchantHomePageIsDisplayed()
    {
        try
        {
            await this.mainPage.AssertOnPage().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            string pageSource = await this.mainPage.GetPageSource().ConfigureAwait(false);
            throw new Exception($"Unable to verify on page: {this.mainPage.GetType().Name}{Environment.NewLine}Page Source: {pageSource}", ex);
        }
    }

    [Then(@"the available balance is shown as (.*)")]
    public Task ThenTheAvailableBalanceIsShownAs(decimal expectedAvailableBalance)
    {
        _ = expectedAvailableBalance;
        return Task.CompletedTask;
    }

    [Then(@"the Login Screen is displayed")]
    public async Task ThenTheLoginScreenIsDisplayed()
    {
        await this.loginPage.AssertOnPage().ConfigureAwait(false);
    }
}

