namespace TransactionMobile.Maui.UITests.Steps;

using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TransactionMobile.Maui.UiTests.Common;

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

}