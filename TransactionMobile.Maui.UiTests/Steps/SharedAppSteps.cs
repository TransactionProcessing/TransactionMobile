using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace TransactionMobile.Maui.UiTests.Steps
{
    using Drivers;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Enums;
    using Pages;
    using Shouldly;
    using UITests;

    [Binding]
    [Scope(Tag = "sharedapp")]
    public class SharedAppSteps
    {
        private SharedPage sharedPage = new SharedPage();

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
            AppState state = AppiumDriverWrapper.Driver.GetAppState("com.transactionprocessing.pos");
            state.ShouldBe(AppState.NotRunning);
        }

        [When(@"I click yes")]
        public async Task WhenIClickYes() {
            await this.sharedPage.AcceptAlert();
        }

        [When(@"I click no")]
        public async Task WhenIClickNo()
        {
            await this.sharedPage.DismissAlert();
        }

        [Then(@"A message is displayed confirming I want to log out")]
        public async Task ThenAMessageIsDisplayedConfirmingIWantToLogOut()
        {
            await this.sharedPage.LogoutMessageIsDisplayed("Title", "Logout Message");
        }
    }
}
