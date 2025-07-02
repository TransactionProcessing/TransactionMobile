using OpenQA.Selenium.Appium.Enums;
using Reqnroll;
using Shouldly;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps
{
    [Binding]
    [Scope(Tag = "sharedapp")]
    public class SharedAppSteps
    {
        public SharedAppSteps(TestingContext testingContext){
            this.sharedPage = new SharedPage(testingContext);
        }

        private SharedPage sharedPage;

        [When(@"I click on the device back button")]
        public void WhenIClickOnTheDeviceBackButton()
        {
            this.sharedPage.NavigateBack();
        }

        [When(@"I click on the back button")]
        public async Task WhenIClickOnTheBackButton() {
            await this.sharedPage.ClickBackButton();
        }

        [Then(@"The application closes")]
        public void ThenTheApplicationCloses() {
            if (AppiumDriverWrapper.MobileTestPlatform != MobileTestPlatform.iOS) {
                AppState state = AppiumDriverWrapper.Driver.GetAppState("com.transactionprocessor.mobile");
                state.ShouldBe(AppState.NotRunning);
            }
        }

        [When(@"I click yes")]
        public async Task WhenIClickYes(){
            await this.sharedPage.AcceptAlert();
        }

        [When(@"I click no")]
        public async Task WhenIClickNo(){
            await this.sharedPage.DismissAlert();
        }

        [Then(@"A message is displayed confirming I want to log out")]
        public async Task ThenAMessageIsDisplayedConfirmingIWantToLogOut()
        {
            await this.sharedPage.LogoutMessageIsDisplayed("Title", "Logout Message");
        }
    }
}
