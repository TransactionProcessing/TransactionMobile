namespace TransactionMobile.Maui.UiTests.Pages;

using OpenQA.Selenium;
using System;
using System.Threading.Tasks;
using Shared.IntegrationTesting;
using TransactionMobile.Maui.UiTests.Common;
using UITests;

public class TransactionsMobileTopupSelectOperatorPage : BasePage2
{
    public TransactionsMobileTopupSelectOperatorPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "Select an Operator";

    #endregion

    public async Task ClickOperatorButton(String operatorName) {
        IWebElement element = await this.WaitForElementByAccessibilityId(operatorName);
        element.Click();
    }
}

public class TransactionsMobileTopupSelectProductPage : BasePage {
    public TransactionsMobileTopupSelectProductPage(TestingContext testingContext) : base(testingContext) {

    }

    #region Properties

    protected override String Trait => "Select a Product";

    #endregion

    public async Task ClickProductButton(String productText) {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}

public class TransactionsMobileTopupEnterTopupDetailsPage : BasePage {

    private readonly String CustomerMobileNumberEntry;
    private readonly String TopupAmountEntry;
    private readonly String CustomerEmailAddressEntry;
    private readonly String PerformTopupButton;

    public TransactionsMobileTopupEnterTopupDetailsPage(TestingContext testingContext) : base(testingContext){
        this.CustomerMobileNumberEntry = "CustomerMobileNumberEntry";
        this.TopupAmountEntry = "TopupAmountEntry";
        this.CustomerEmailAddressEntry = "CustomerEmailAddressEntry";
        this.PerformTopupButton = "PerformTopupButton";
    }

    #region Properties

    protected override String Trait => "Enter Topup Details";

    public async Task EnterCustomerMobileNumber(String customerMobileNumber) {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.CustomerMobileNumberEntry);

        element.SendKeys(customerMobileNumber);
    }

    public async Task EnterTopupAmount(String topupAmount) {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.TopupAmountEntry);

        element.SendKeys(topupAmount);
    }

    public async Task EnterCustomerEmailAddress(String customerEmailAddress) {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

        element.SendKeys(customerEmailAddress);
    }

    public async Task ClickPerformTopupButton() {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.PerformTopupButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}

public class TransactionsMobileTopupSuccessfulTopupPage : BasePage {

    private readonly String CompleteButton;

    public TransactionsMobileTopupSuccessfulTopupPage(TestingContext testingContext) : base(testingContext) {
        this.CompleteButton = "CompleteButton";
    }

    #region Properties

    protected override String Trait => "Mobile Topup Successful";
    
    public async Task ClickCompleteButton() {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.CompleteButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}

public class TransactionsVoucherSelectOperatorPage : BasePage2
{
    public TransactionsVoucherSelectOperatorPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "Select an Operator";

    #endregion

    public async Task ClickOperatorButton(String operatorName)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(operatorName);
        element.Click();
    }
}

public class TransactionsVoucherSelectProductPage : BasePage
{
    public TransactionsVoucherSelectProductPage(TestingContext testingContext) : base(testingContext)
    {

    }

    #region Properties

    protected override String Trait => "Select a Product";

    #endregion

    public async Task ClickProductButton(String productText)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}

public class TransactionsVoucherEnterVoucherIssueDetailsPage : BasePage
{

    private readonly String RecipientMobileNumberEntry;
    private readonly String RecipientEmailAddressEntry;
    private readonly String VoucherAmountEntry;
    private readonly String CustomerEmailAddressEntry;
    private readonly String IssueVoucherButton;

    public TransactionsVoucherEnterVoucherIssueDetailsPage(TestingContext testingContext) : base(testingContext)
    {
        this.RecipientMobileNumberEntry = "RecipientMobileNumberEntry";
        this.RecipientEmailAddressEntry = "RecipientEmailAddressEntry";
        this.VoucherAmountEntry = "VoucherAmountEntry";
        this.CustomerEmailAddressEntry = "CustomerEmailAddressEntry";
        this.IssueVoucherButton = "IssueVoucherButton";
    }

    #region Properties

    protected override String Trait => "Enter Voucher Issue Details";

    public async Task EnterRecipientMobileNumber(String recipientMobileNumber)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.RecipientMobileNumberEntry);

        element.SendKeys(recipientMobileNumber);
    }

    public async Task EnterRecipientEmailAddress(String recipientEmailAddress)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.RecipientEmailAddressEntry);

        element.SendKeys(recipientEmailAddress);
    }

    public async Task EnterVoucherAmount(String voucherAmount)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.VoucherAmountEntry);

        element.SendKeys(voucherAmount);
    }

    public async Task EnterCustomerEmailAddress(String customerEmailAddress)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.CustomerEmailAddressEntry);

        element.SendKeys(customerEmailAddress);
    }

    public async Task ClickIssueVoucherButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.IssueVoucherButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}


public class TransactionsVoucherIssueSuccessfulTopupPage : BasePage
{

    private readonly String CompleteButton;

    public TransactionsVoucherIssueSuccessfulTopupPage(TestingContext testingContext) : base(testingContext)
    {
        this.CompleteButton = "CompleteButton";
    }

    #region Properties

    protected override String Trait => "Voucher Issue Successful";

    public async Task ClickCompleteButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.CompleteButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }

    #endregion
}