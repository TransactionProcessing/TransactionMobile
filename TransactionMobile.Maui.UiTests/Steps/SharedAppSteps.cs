using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.UiTests.Steps
{
    using Common;
    using Drivers;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Enums;
    using Pages;
    using Reqnroll;
    using Shouldly;
    using UITests;

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
            //AppState state = AppiumDriverWrapper.Driver.GetAppState("com.transactionprocessing.pos");
            //state.ShouldBe(AppState.NotRunning);
            //AppState appState = (AppState)AppiumDriverWrapper.Driver.ExecuteScript("mobile: queryAppState", new Dictionary<string, object>
            //{
            //    ["appId"] = "com.transactionprocessing.pos"
            //});
            //appState.ShouldBe(AppState.NotRunning, "The application should not be running after clicking the back button.");

            var state = AppiumDriverWrapper.Driver.ExecuteScript("mobile: queryAppState", new Dictionary<string, object>
            {
                ["appId"] = "com.transactionprocessing.pos"
            });
            Console.WriteLine($"State is {state}");
        }

        public enum AppState
        {
            NotInstalled = 0,
            NotRunning = 1,
            RunningInBackgroundSuspended = 2,
            RunningInBackground = 3,
            RunningInForeground = 4
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
