using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.IntegrationTesting.Helpers;

namespace TransactionMobile.Maui.UITests.Steps
{
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Reqnroll;
    using Shared.IntegrationTesting;
    using Shouldly;
    using TransactionMobile.Maui.UiTests.Common;

    [Binding]
    [Scope(Tag = "login")]
    public class LoginSteps
    {
        private readonly TestingContext TestingContext;

        private LoginPage loginPage;

        private MainPage mainPage;

        public LoginSteps(TestingContext testingContext) {
            this.TestingContext = testingContext;
            this.loginPage = new LoginPage(testingContext);
            this.mainPage = new MainPage(testingContext);
        }

        [Given(@"I am on the Login Screen")]
        [Then("the Login Page is displayed")]
        public async Task GivenIAmOnTheLoginScreen() {
            await this.loginPage.AssertOnPage();
        }

        [Given(@"my device is registered")]
        public async Task GivenMyDeviceIsRegistered() {
            String configHostUrl = $"http://{this.TestingContext.DockerHelper.LocalIPAddress}:{this.TestingContext.DockerHelper.ConfigHostPort}";
            await this.loginPage.SetConfigHostUrl(configHostUrl);

            String serial = await this.loginPage.GetDeviceSerial();

            AddMerchantDeviceRequest request = new AddMerchantDeviceRequest {
                                                                                DeviceIdentifier = serial
                                                                            };
            Guid estateId = this.TestingContext.GetAllEstateIds().Single();
            EstateDetails estate = this.TestingContext.GetEstateDetails(estateId);
            Guid merchantId = estate.GetMerchantId("Test Merchant 1"); // TODO: stop this from being hard coded
            await this.TestingContext.DockerHelper.TransactionProcessorClient.AddDeviceToMerchant(this.TestingContext.AccessToken, estateId, merchantId, request, CancellationToken.None);
        }
        
        [Given(@"the application is in training mode")]
        public async Task GivenTheApplicationIsInTrainingMode() {
            var isTrainingModeOn = await this.loginPage.IsTrainingModeOn();

            if (isTrainingModeOn == false)
                await this.loginPage.SetTrainingModeOn();
        }

        [When(@"I enter '(.*)' as the Email Address")]
        public async Task WhenIEnterAsTheEmailAddress(String emailAddress) {
            //Char[] x = emailAddress.ToCharArray();
            //StringBuilder expected = new StringBuilder();
            //foreach (Char c in x){
            //    expected.Append(c);
            //    await this.loginPage.EnterEmailAddress(c.ToString());
            //    await Task.Delay(2000);
            //    String text = await this.loginPage.GetEmailAddress();
            //    text.ShouldBe(expected.ToString());
            //}
            await this.loginPage.EnterEmailAddress(emailAddress);
            await Task.Delay(2000);
            String text = await this.loginPage.GetEmailAddress();
            text.ShouldBe(emailAddress);
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
