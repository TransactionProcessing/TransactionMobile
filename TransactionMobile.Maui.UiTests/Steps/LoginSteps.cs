using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UITests.Steps
{
    using System.Threading;
    using EstateManagement.DataTransferObjects.Requests;
    using TechTalk.SpecFlow;
    using TransactionMobile.Maui.UiTests.Common;

    [Binding]
    [Scope(Tag = "login")]
    public class LoginSteps
    {
        private readonly TestingContext TestingContext;

        LoginPage loginPage = new LoginPage();

        MainPage mainPage = new MainPage();

        public LoginSteps(TestingContext testingContext) {
            this.TestingContext = testingContext;
        }

        [Given(@"I am on the Login Screen")]
        [Then("the Login Page is displayed")]
        public async Task GivenIAmOnTheLoginScreen() {
            await this.loginPage.AssertOnPage();
        }

        [Given(@"my device is registered")]
        public async Task GivenMyDeviceIsRegistered() {
            String serial = await this.loginPage.GetDeviceSerial();
            AddMerchantDeviceRequest request = new AddMerchantDeviceRequest {
                                                                                DeviceIdentifier = serial
                                                                            };
            Guid estateId = this.TestingContext.GetAllEstateIds().Single();
            EstateDetails estate = this.TestingContext.GetEstateDetails(estateId);
            Guid merchantId = estate.GetAllMerchantIds().Single();
            await this.TestingContext.DockerHelper.EstateClient.AddDeviceToMerchant(this.TestingContext.AccessToken, estateId, merchantId, request, CancellationToken.None);
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
