namespace TransactionMobile.Maui.UITests.Steps;

using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using UiTests.Common;
using UiTests.Pages;

[Binding]
[Scope(Tag = "transactions")]
public class TransactionsSteps
{
    private readonly TestingContext TestingContext;

    private TransactionsPage transactionsPage;

    private TransactionsMobileTopupSelectOperatorPage transactionsMobileTopupSelectOperatorPage;
    private TransactionsMobileTopupSelectProductPage transactionsMobileTopupSelectProductPage;
    private TransactionsMobileTopupEnterTopupDetailsPage transactionsMobileTopupEnterTopupDetailsPage;
    private TransactionsMobileTopupSuccessfulTopupPage transactionsMobileTopupSuccessfulTopupPage;

    public TransactionsSteps(TestingContext testingContext){
        this.TestingContext = testingContext;
        this.transactionsPage = new TransactionsPage(testingContext);
        this.transactionsMobileTopupSelectOperatorPage = new TransactionsMobileTopupSelectOperatorPage(testingContext);
        this.transactionsMobileTopupSelectProductPage = new TransactionsMobileTopupSelectProductPage(testingContext);
        this.transactionsMobileTopupEnterTopupDetailsPage = new TransactionsMobileTopupEnterTopupDetailsPage(testingContext);
        this.transactionsMobileTopupSuccessfulTopupPage = new TransactionsMobileTopupSuccessfulTopupPage(testingContext);
    }

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
        await this.transactionsMobileTopupSelectOperatorPage.AssertOnPage();
    }

    [When(@"I tap on the '([^']*)' button")]
    public async Task WhenITapOnTheButton(String operatorName) {
        await this.transactionsMobileTopupSelectOperatorPage.ClickOperatorButton(operatorName);
    }
    
    [Then(@"the Select Product Page is displayed")]
    public async Task ThenTheSelectProductPageIsDisplayed() {
        await this.transactionsMobileTopupSelectProductPage.AssertOnPage();
    }

    [When(@"I tap on the '([^']*)' product button")]
    public async Task WhenITapOnTheProductButton(String productText) {
        await this.transactionsMobileTopupSelectProductPage.ClickProductButton(productText);
    }
    
    [Then(@"the Enter Topup Details Page is displayed")]
    public async Task ThenTheEnterTopupDetailsPageIsDisplayed() {
        await this.transactionsMobileTopupEnterTopupDetailsPage.AssertOnPage();
    }

    [When(@"I enter '([^']*)' as the Customer Mobile Number")]
    public async Task WhenIEnterAsTheCustomerMobileNumber(string customerMobileNumber){
        await this.transactionsMobileTopupEnterTopupDetailsPage.EnterCustomerMobileNumber(customerMobileNumber);
    }

    [When(@"I enter (.*) as the Topup Amount")]
    public async Task WhenIEnterAsTheTopupAmount(Decimal topupAmount) {
        await this.transactionsMobileTopupEnterTopupDetailsPage.EnterTopupAmount(topupAmount.ToString());
    }

    [When(@"I tap on Perform Topup")]
    public async Task WhenITapOnPerformTopup(){
        await this.transactionsMobileTopupEnterTopupDetailsPage.ClickPerformTopupButton();
    }

    [Then(@"the Mobile Topup Successful Page is displayed")]
    public async Task ThenTheMobileTopupSuccessfulPageIsDisplayed() {
        await this.transactionsMobileTopupSuccessfulTopupPage.AssertOnPage();
    }

    [Then(@"I tap on Complete")]
    public async Task ThenITapOnComplete(){
        await this.transactionsMobileTopupSuccessfulTopupPage.ClickCompleteButton();
    }

}