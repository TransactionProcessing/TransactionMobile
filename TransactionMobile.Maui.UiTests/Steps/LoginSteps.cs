using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UITests.Steps
{
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "login")]
    public class LoginSteps
    {
        LoginPage loginPage = new LoginPage();

        MainPage mainPage = new MainPage();

        [Given(@"I am on the Login Screen")]
        [Then("the Login Page is displayed")]
        public async Task GivenIAmOnTheLoginScreen() {
            await this.loginPage.AssertOnPage();
        }

        [Given(@"the application is in training mode")]
        public async Task GivenTheApplicationIsInTrainingMode() {
            var isTrainingModeOn = await this.loginPage.IsTrainingModeOn();

            if (isTrainingModeOn == false)
                await this.loginPage.SetTrainingModeOn();
        }

        [When(@"I enter '(.*)' as the Email Address")]
        public async Task WhenIEnterAsTheEmailAddress(String emailAddress) {
            await this.loginPage.EnterEmailAddress(emailAddress);
        }

        [When(@"I enter '(.*)' as the Password")]
        public async Task WhenIEnterAsThePassword(String password) {
            await this.loginPage.EnterPassword(password);
        }

        [When(@"I tap on Login")]
        public async Task WhenITapOnLogin() {
            await this.loginPage.ClickLoginButton();
        }

        [Then(@"the Merchant Home Page is displayed")]
        public async Task ThenTheMerchantHomePageIsDisplayed() {
            await this.mainPage.AssertOnPage();
        }

        [Then(@"the available balance is shown as (.*)")]
        public async Task ThenTheAvailableBalanceIsShownAs(Decimal expectedAvailableBalance) {
            //Decimal availableBalance = await this.mainPage.GetAvailableBalanceValue(TimeSpan.FromSeconds(120)).ConfigureAwait(false);
            //availableBalance.ShouldBe(expectedAvailableBalance);
        }

        [Then(@"the Login Screen is displayed")]
        public async Task ThenTheLoginScreenIsDisplayed() {
            await this.loginPage.AssertOnPage();
        }
    }
}
