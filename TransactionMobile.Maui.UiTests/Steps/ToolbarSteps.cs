namespace TransactionMobile.Maui.UITests.Steps;

using System.Threading.Tasks;
using TechTalk.SpecFlow;

[Binding]
[Scope(Tag = "toolbar")]
public class ToolbarSteps
{
    MainPage mainPage = new MainPage();

    [When(@"I tap on Profile")]
    public async Task WhenITapOnProfile() {
        await this.mainPage.ClickProfileButton();
    }

    [When(@"I tap on Transactions")]
    public async Task WhenITapOnTransactions() {
        await this.mainPage.ClickTransactionsButton();
    }

}