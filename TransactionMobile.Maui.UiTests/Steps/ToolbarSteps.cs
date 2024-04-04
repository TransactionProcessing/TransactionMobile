namespace TransactionMobile.Maui.UITests.Steps;

using System.Threading.Tasks;
using Reqnroll;
using TransactionMobile.Maui.UiTests.Common;
using UiTests.Pages;

[Binding]
[Scope(Tag = "toolbar")]
public class ToolbarSteps{
    private MainPage mainPage;
    private readonly TestingContext TestingContext;
    public ToolbarSteps(TestingContext testingContext){
        this.TestingContext = testingContext;
        this.mainPage = new MainPage(testingContext);
    }

    [When(@"I tap on Profile")]
    public async Task WhenITapOnProfile() {
        await this.mainPage.ClickProfileButton();
    }

    [When(@"I tap on Transactions")]
    public async Task WhenITapOnTransactions() {
        await this.mainPage.ClickTransactionsButton();
    }

    [When(@"I tap on Reports")]
    public async Task WhenITapOnReports(){
        await this.mainPage.ClickReportsButton();
    }

    [When(@"I tap on Support")]
    public async Task WhenITapOnSupport()
    {
        await this.mainPage.ClickSupportButton();
    }
}

[Binding]
[Scope(Tag = "support")]
public class SupportSteps{
    private readonly TestingContext TestingContext;

    private SupportPage supportPage;
    private ViewLogsPage viewLogsPage;

    public SupportSteps(TestingContext testingContext){
        this.TestingContext = testingContext;
        this.supportPage = new SupportPage(testingContext);
        this.viewLogsPage = new ViewLogsPage(testingContext);
    }

    [Then(@"the Support Page is displayed")]
    public async Task ThenTheSupportPageIsDisplayed(){
        await this.supportPage.AssertOnPage();
    }

    [When(@"I tap on the Upload Logs Button")]
    public async Task WhenITapOnTheUploadLogsButton()
    {
        await this.supportPage.ClickUploadLogsButton();
    }

    [When(@"I tap on the View Logs Button")]
    public async Task WhenITapOnTheViewLogsButton(){
        await this.supportPage.ClickViewLogsButton();
    }

    [Then(@"the View Logs Page is displayed")]
    public async Task ThenTheViewLogsPageIsDisplayed()
    {
        await this.viewLogsPage.AssertOnPage();
    }

}