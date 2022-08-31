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
    [Scope(Tag = "shared")]
    public class SharedSteps
    {
        private SharedPage sharedPage = new SharedPage();

        [When(@"I click on the back button")]
        public void WhenIClickOnTheBackButton()
        {
            this.sharedPage.NavigateBack();
        }

        [Then(@"The application closes")]
        public void ThenTheApplicationCloses() {
            AppState state = AppiumDriverWrapper.Driver.GetAppState("com.transactionprocessing.pos");
            state.ShouldBe(AppState.RunningInBackground);
        }

        [When(@"I click yes")]
        public void WhenIClickYes() {
            this.sharedPage.AcceptAlert();
        }

        [When(@"I click no")]
        public void WhenIClickNo()
        {
            this.sharedPage.DismissAlert();
        }

        [Then(@"A message is displayed confirming I want to log out")]
        public void ThenAMessageIsDisplayedConfirmingIWantToLogOut()
        {
            this.sharedPage.LogoutMessageIsDisplayed("Title", "Logout Message");
        }
    }
}
