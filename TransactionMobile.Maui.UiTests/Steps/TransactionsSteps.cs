namespace TransactionMobile.Maui.UITests.Steps;

using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using UiTests.Common;
using UiTests.Pages;

[Binding]
[Scope(Tag = "transactions")]
public class TransactionsSteps{
    public Int32 operatorType = 0;

    private readonly TestingContext TestingContext;

    private TransactionsPage transactionsPage;

    private TransactionsMobileTopupSelectOperatorPage transactionsMobileTopupSelectOperatorPage;
    private TransactionsMobileTopupSelectProductPage transactionsMobileTopupSelectProductPage;
    private TransactionsMobileTopupEnterTopupDetailsPage transactionsMobileTopupEnterTopupDetailsPage;
    private TransactionsMobileTopupSuccessfulTopupPage transactionsMobileTopupSuccessfulTopupPage;
    private TransactionsVoucherSelectOperatorPage transactionsVoucherSelectOperatorPage;
    private TransactionsVoucherSelectProductPage transactionsVoucherSelectProductPage;
    private TransactionsVoucherEnterVoucherIssueDetailsPage transactionsVoucherEnterVoucherIssueDetailsPage;
    private TransactionsVoucherIssueSuccessfulTopupPage transactionsVoucherIssueSuccessfulTopupPage;

    public TransactionsSteps(TestingContext testingContext){
        this.TestingContext = testingContext;
        this.transactionsPage = new TransactionsPage(testingContext);
        this.transactionsMobileTopupSelectOperatorPage = new TransactionsMobileTopupSelectOperatorPage(testingContext);
        this.transactionsMobileTopupSelectProductPage = new TransactionsMobileTopupSelectProductPage(testingContext);
        this.transactionsMobileTopupEnterTopupDetailsPage = new TransactionsMobileTopupEnterTopupDetailsPage(testingContext);
        this.transactionsMobileTopupSuccessfulTopupPage = new TransactionsMobileTopupSuccessfulTopupPage(testingContext);
        this.transactionsVoucherSelectOperatorPage = new TransactionsVoucherSelectOperatorPage(testingContext);
        this.transactionsVoucherSelectProductPage = new TransactionsVoucherSelectProductPage(testingContext);
        this.transactionsVoucherEnterVoucherIssueDetailsPage = new TransactionsVoucherEnterVoucherIssueDetailsPage(testingContext);
        this.transactionsVoucherIssueSuccessfulTopupPage = new TransactionsVoucherIssueSuccessfulTopupPage(testingContext);
    }

    [Then(@"the Transaction Page is displayed")]
    public async Task ThenTheTransactionPageIsDisplayed() {
        await this.transactionsPage.AssertOnPage();
    }

    [When(@"I tap on the Mobile Topup button")]
    public async Task WhenITapOnTheMobileTopupButton(){
        operatorType = 1;
        await this.transactionsPage.ClickMobileTopupButton();
    }

    [When(@"I tap on the Voucher button")]
    public async Task WhenITapOnTheVoucherButton(){
        operatorType = 2;
        await this.transactionsPage.ClickVoucherButton();
    }

    [Then(@"the Enter Voucher Issue Details Page is displayed")]
    public async Task ThenTheEnterVoucherIssueDetailsPageIsDisplayed(){
        await this.transactionsVoucherEnterVoucherIssueDetailsPage.AssertOnPage();
    }

    [When(@"I enter '([^']*)' as the Recipient Mobile Number")]
    public async Task WhenIEnterAsTheRecipientMobileNumber(string recipientMobileNumber){
        await this.transactionsVoucherEnterVoucherIssueDetailsPage.EnterRecipientMobileNumber(recipientMobileNumber);
    }
    
    [When(@"I tap on Issue Voucher")]
    public async Task WhenITapOnIssueVoucher(){
        await this.transactionsVoucherEnterVoucherIssueDetailsPage.ClickIssueVoucherButton();
    }

    [Then(@"the Voucher Issue Successful Page is displayed")]
    public async Task ThenTheVoucherIssueSuccessfulPageIsDisplayed()
    {
        await this.transactionsVoucherIssueSuccessfulTopupPage.AssertOnPage();
    }

    [Then(@"the Transaction Select Mobile Topup Operator Page is displayed")]
    public async Task ThenTheTransactionSelectMobileTopupOperatorPageIsDisplayed() {
        await this.transactionsMobileTopupSelectOperatorPage.AssertOnPage();
    }

    [Then(@"the Transaction Select Voucher Operator Page is displayed")]
    public async Task ThenTheTransactionSelectVoucherOperatorPageIsDisplayed(){
        await this.transactionsVoucherSelectOperatorPage.AssertOnPage();
    }
    
    [When(@"I tap on the '([^']*)' button")]
    public async Task WhenITapOnTheButton(String operatorName) {

        if (this.operatorType == 1){
            await this.transactionsMobileTopupSelectOperatorPage.ClickOperatorButton(operatorName);
        }
        else{
            await this.transactionsVoucherSelectOperatorPage.ClickOperatorButton(operatorName);
        }
        
    }
    
    [Then(@"the Select Product Page is displayed")]
    public async Task ThenTheSelectProductPageIsDisplayed() {
        await this.transactionsMobileTopupSelectProductPage.AssertOnPage();
    }

    [When(@"I tap on the '([^']*)' product button")]
    public async Task WhenITapOnTheProductButton(String productText) {
        if (this.operatorType == 1){
            await this.transactionsMobileTopupSelectProductPage.ClickProductButton(productText);
        }
        else{
            await this.transactionsVoucherSelectProductPage.ClickProductButton(productText);
        }
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
        if (this.operatorType == 1){
            await this.transactionsMobileTopupSuccessfulTopupPage.ClickCompleteButton();
        }
        else{
            await this.transactionsVoucherIssueSuccessfulTopupPage.ClickCompleteButton();
        }
    }

}