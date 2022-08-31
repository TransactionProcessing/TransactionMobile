namespace TransactionMobile.Maui.UITests.Steps;

using System.Threading.Tasks;
using TechTalk.SpecFlow;
using UiTests.Pages;

[Binding]
[Scope(Tag = "transactions")]
public class TransactionsSteps
{
    private TransactionsPage transactionsPage = new TransactionsPage();

    private TransactionsSelectMobileTopupOperatorPage transactionsSelectMobileTopupOperatorPage = new TransactionsSelectMobileTopupOperatorPage();

    [Then(@"the Transaction Page is displayed")]
    public async Task ThenTheTransactionPageIsDisplayed() {
        await this.transactionsPage.AssertOnPage();
    }

    [When(@"I tap on the Mobile Topup button")]
    public async Task WhenITapOnTheMobileTopupButton() {
        await this.transactionsPage.ClickMobileTopupButton();
    }

    [Then(@"the Transaction Select Mobile Topup Operator Page is displayed")]
    public async Task ThenTheTransactionSelectMobileTopupOperatorPageIsDisplayed() {
        await this.transactionsSelectMobileTopupOperatorPage.AssertOnPage();
    }

}