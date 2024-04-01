namespace TransactionMobile.Maui.UITests.Steps;

using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using OpenQA.Selenium.DevTools.V118.Network;
using Reqnroll;
using Shared.IntegrationTesting;
using Shouldly;
using UiTests.Common;
using UiTests.Pages;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Object = System.Object;
using String = System.String;

public enum OperatorType{
    NotSet,
    MobileTopup,
    Voucher,
    BillPayment
}

public enum BillPaymentType
{
    NotSet,
    PostPayment,
    PrePayment
}

[Binding]
[Scope(Tag = "transactions")]
public class TransactionsSteps{
    public OperatorType operatorType;

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
    private TransactionsBillPaymentSelectOperatorPage transactionsBillPaymentSelectOperatorPage;
    private TransactionsBillPaymentSelectProductPage transactionsBillPaymentSelectProductPage;
    private TransactionsBillPaymentEnterAccountDetailsPage transactionsBillPaymentEnterAccountDetailsPage;
    private TransactionsBillPaymentEnterMeterDetailsPage transactionsBillPaymentEnterMeterDetailsPage;
    private TransactionsBillPaymentMakeAPaymentPage transactionsBillPaymentMakeAPaymentPage;
    private TransactionsBillPaymentSuccessfulPaymentPage transactionsBillPaymentSuccessfulPaymentPage;

    private BillPaymentType BillPaymentType;

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
        this.transactionsBillPaymentSelectOperatorPage = new TransactionsBillPaymentSelectOperatorPage(testingContext);
        this.transactionsBillPaymentSelectProductPage = new TransactionsBillPaymentSelectProductPage(testingContext);
        this.transactionsBillPaymentEnterAccountDetailsPage = new TransactionsBillPaymentEnterAccountDetailsPage(testingContext);
        this.transactionsBillPaymentEnterMeterDetailsPage = new TransactionsBillPaymentEnterMeterDetailsPage(testingContext);
        this.transactionsBillPaymentMakeAPaymentPage = new TransactionsBillPaymentMakeAPaymentPage(testingContext);
        this.transactionsBillPaymentSuccessfulPaymentPage = new TransactionsBillPaymentSuccessfulPaymentPage(testingContext);
    }

    [Then(@"the Transaction Page is displayed")]
    public async Task ThenTheTransactionPageIsDisplayed() {
        await this.transactionsPage.AssertOnPage();
    }

    [When(@"I tap on the Mobile Topup button")]
    public async Task WhenITapOnTheMobileTopupButton(){
        operatorType = OperatorType.MobileTopup;
        await this.transactionsPage.ClickMobileTopupButton();
    }

    [When(@"I tap on the Voucher button")]
    public async Task WhenITapOnTheVoucherButton(){
        operatorType = OperatorType.Voucher;
        await this.transactionsPage.ClickVoucherButton();
    }

    [When(@"I tap on the Bill Payment button")]
    public async Task WhenITapOnTheBillPaymentButton()
    { operatorType = OperatorType.BillPayment;
        await this.transactionsPage.ClickBillPaymentButton();
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

    [Then(@"the Transaction Select Bill Payment Operator Page is displayed")]
    public async Task ThenTheTransactionSelectBillPaymentOperatorPageIsDisplayed(){
        await this.transactionsBillPaymentSelectOperatorPage.AssertOnPage();
    }


    [When(@"I tap on the '([^']*)' button")]
    public async Task WhenITapOnTheButton(String operatorName){

        Task t = this.operatorType switch{
            OperatorType.MobileTopup => this.transactionsMobileTopupSelectOperatorPage.ClickOperatorButton(operatorName),
            OperatorType.Voucher => this.transactionsVoucherSelectOperatorPage.ClickOperatorButton(operatorName),
            OperatorType.BillPayment => this.transactionsBillPaymentSelectOperatorPage.ClickOperatorButton(operatorName)
        };

        await t;
    }
    
    [Then(@"the Select Product Page is displayed")]
    public async Task ThenTheSelectProductPageIsDisplayed() {
        await this.transactionsMobileTopupSelectProductPage.AssertOnPage();
    }

    [When(@"I tap on the '([^']*)' product button")]
    public async Task WhenITapOnTheProductButton(String productText){

        Task t = this.operatorType switch{
            OperatorType.MobileTopup => this.transactionsMobileTopupSelectProductPage.ClickProductButton(productText),
            OperatorType.Voucher => this.transactionsVoucherSelectProductPage.ClickProductButton(productText),
            OperatorType.BillPayment => this.transactionsBillPaymentSelectProductPage.ClickProductButton(productText)
        };

        await t;
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
        Task t = this.operatorType switch{
            OperatorType.MobileTopup => this.transactionsMobileTopupSuccessfulTopupPage.ClickCompleteButton(),
            OperatorType.Voucher => this.transactionsVoucherIssueSuccessfulTopupPage.ClickCompleteButton(),
            OperatorType.BillPayment=> this.transactionsBillPaymentSuccessfulPaymentPage.ClickCompleteButton()
        };

        await t;
    }

    [Then(@"the Enter Account Details Page is displayed")]
    public async Task ThenTheEnterAccountDetailsPageIsDisplayed(){
        await this.transactionsBillPaymentEnterAccountDetailsPage.AssertOnPage();
    }
    
    [When(@"I enter '([^']*)' as the Account Number")]
    public async Task WhenIEnterAsTheAccountNumber(String accountNumber){
        await this.transactionsBillPaymentEnterAccountDetailsPage.EnterCustomerAccountNumber(accountNumber);
    }

    [When(@"I tap on the Get Account Button")]
    public async Task WhenITapOnTheGetAccountButton(){
        await this.transactionsBillPaymentEnterAccountDetailsPage.ClickGetAccountButton();
    }

    [Then(@"the Make Bill Payment page is displayed")]
    public async Task ThenTheMakeBillPaymentPageIsDisplayed(){
        await this.transactionsBillPaymentMakeAPaymentPage.AssertOnPage();
    }

    [Then(@"the following Bill Details are displayed")]
    public async Task ThenTheFollowingBillDetailsAreDisplayed(DataTable table){
        String accountNumber = await this.transactionsBillPaymentMakeAPaymentPage.GetAccountNumberValue();
        String accountHolder = await this.transactionsBillPaymentMakeAPaymentPage.GetAccountHolderValue();
        String balance = await this.transactionsBillPaymentMakeAPaymentPage.GetBalanceValue();
        String dueDate= await this.transactionsBillPaymentMakeAPaymentPage.GetDueDateValue();

        DataTableRow? tableRow = table.Rows.Single();
        String expectedAccountNumber = ReqnrollTableHelper.GetStringRowValue(tableRow, "AccountNumber");
        String expectedAccountHolder = ReqnrollTableHelper.GetStringRowValue(tableRow, "AccountHolder");
        String expectedBalance =  ReqnrollTableHelper.GetStringRowValue(tableRow,"Balance");
        String dueDateValue = ReqnrollTableHelper.GetStringRowValue(tableRow, "DueDate");
        DateTime expectedBillDueDate = ReqnrollTableHelper.GetDateForDateString(dueDateValue, DateTime.Now);
        
        accountNumber.ShouldBe($"Account Number: {expectedAccountNumber}");
        accountHolder.ShouldBe($"Account Holder: {expectedAccountHolder}");
        balance.ShouldBe($"Balance: {expectedBalance} KES");
        // TODO: Handle BST date changes
        //dueDate.ShouldBe($"Due Date: {expectedBillDueDate:yyyy-MM-dd}");
        this.BillPaymentType = BillPaymentType.PostPayment;
    }

    [When(@"I enter (.*) as the Payment Amount")]
    public async Task WhenIEnterAsThePaymentAmount(Decimal paymentAmount){

        if (this.BillPaymentType == BillPaymentType.PostPayment){
            await this.transactionsBillPaymentMakeAPaymentPage.EnterPostPaymentAmount(paymentAmount.ToString());
        }
        else{
            await this.transactionsBillPaymentMakeAPaymentPage.EnterPrePaymentAmount(paymentAmount.ToString());
        }
    }

    [When(@"I tap on the Make Payment Button")]
    public async Task WhenITapOnTheMakePaymentButton(){
        await this.transactionsBillPaymentMakeAPaymentPage.ClickMakePaymentButton();
    }

    [Then(@"the Bill Payment Successful Page is displayed")]
    public async Task ThenTheBillPaymentSuccessfulPageIsDisplayed(){
        await this.transactionsBillPaymentSuccessfulPaymentPage.AssertOnPage();
    }

    [Then(@"the Enter Meter Details Page is displayed")]
    public async Task ThenTheEnterMeterDetailsPageIsDisplayed()
    {
        await this.transactionsBillPaymentEnterMeterDetailsPage.AssertOnPage();
    }

    [When(@"I enter '([^']*)' as the Meter Number")]
    public async Task WhenIEnterAsTheMeterNumber(string meterNumber){
        await this.transactionsBillPaymentEnterMeterDetailsPage.EnterMeterNumber(meterNumber);
    }

    [When(@"I tap on the Get Meter Button")]
    public async Task WhenITapOnTheGetMeterButton(){
        await this.transactionsBillPaymentEnterMeterDetailsPage.ClickGetMeterButton();
    }

    [Then(@"the following Meter Details are displayed")]
    public async Task ThenTheFollowingMeterDetailsAreDisplayed(DataTable table)
    {
        String meterNumber = await this.transactionsBillPaymentMakeAPaymentPage.GetMeterNumberValue();

        DataTableRow? tableRow = table.Rows.Single();
        String expectedMeterNumber = ReqnrollTableHelper.GetStringRowValue(tableRow, "MeterNumber");
        
        meterNumber.ShouldBe($"Meter Number: {expectedMeterNumber}");

        this.BillPaymentType = BillPaymentType.PrePayment;
    }

}
