namespace TransactionMobile.Maui.UiTests.Pages;

using System;
using System.Threading.Tasks;
using Common;
using OpenQA.Selenium;
using Shared.IntegrationTesting;
using UITests;

public class TransactionsBillPaymentSelectOperatorPage : BasePage2
{
    public TransactionsBillPaymentSelectOperatorPage(TestingContext testingContext) : base(testingContext)
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

public class TransactionsBillPaymentSelectProductPage : BasePage2{
    public TransactionsBillPaymentSelectProductPage(TestingContext testingContext) : base(testingContext){
    }

    protected override String Trait{ get; }

    public async Task ClickProductButton(String productText)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(productText);
        element.Click();
    }
}

public class TransactionsBillPaymentEnterAccountDetailsPage : BasePage2
{
    private readonly String CustomerAccountNumberEntry;

    private readonly String GetAccountButton;

    public TransactionsBillPaymentEnterAccountDetailsPage(TestingContext testingContext) : base(testingContext)
    {
        this.CustomerAccountNumberEntry = "CustomerAccountNumberEntry";
        this.GetAccountButton = "GetAccountButton";
    }

    protected override String Trait => "Get Customer Account";

    public async Task EnterCustomerAccountNumber(String customerAccountNumber)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.CustomerAccountNumberEntry);

        element.SendKeys(customerAccountNumber);
    }

    public async Task ClickGetAccountButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.GetAccountButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }
}

public class TransactionsBillPaymentEnterMeterDetailsPage : BasePage2
{
    private readonly String MeterNumberEntry;

    private readonly String GetMeterButton;

    public TransactionsBillPaymentEnterMeterDetailsPage(TestingContext testingContext) : base(testingContext)
    {
        this.MeterNumberEntry = "MeterNumberEntry";
        this.GetMeterButton = "GetMeterButton";
    }

    protected override String Trait => "Get Meter";

    public async Task EnterMeterNumber(String meterNumber)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.MeterNumberEntry);

        element.SendKeys(meterNumber);
    }

    public async Task ClickGetMeterButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.GetMeterButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }
}

public class TransactionsBillPaymentMakeAPaymentPage : BasePage2
{
    private readonly String CustomerMobileNumberEntry;
    private readonly String PostPaymentAmountEntry;
    private readonly String PrePaymentAmountEntry;
    private readonly String MakePaymentButton;

    public TransactionsBillPaymentMakeAPaymentPage(TestingContext testingContext) : base(testingContext)
    {
        this.CustomerMobileNumberEntry = "CustomerMobileNumberEntry";
        
        // TODO: handle prepayment as well
        this.PostPaymentAmountEntry = "PostPaymentAmountEntry";
        this.PrePaymentAmountEntry = "PrePaymentAmountEntry";
        this.MakePaymentButton = "MakePaymentButton";
        this.AccountNumberLabel = "AccountNumber";
        this.AccountHolderLabel = "AccountName";
        this.BalanceLabel = "Balance";
        this.DueDateLabel = "DueDate";
        this.MeterNumberLabel = "MeterNumber";

    }

    protected override String Trait => "Make Bill Payment";

    public async Task EnterCustomerMobileNumber(String customerMobileNumber)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.CustomerMobileNumberEntry);

        element.SendKeys(customerMobileNumber);
    }

    public readonly String AccountNumberLabel;
    public readonly String AccountHolderLabel;
    public readonly String BalanceLabel;
    public readonly String DueDateLabel;

    public readonly String MeterNumberLabel;

    public async Task<String> GetAccountNumberValue(){
        return await this.GetLabelValue(this.AccountNumberLabel);
    }
    public async Task<String> GetMeterNumberValue()
    {
        return await this.GetLabelValue(this.MeterNumberLabel);
    }

    public async Task<String> GetAccountHolderValue()
    {
        return await this.GetLabelValue(this.AccountHolderLabel);
    }
    public async Task<String> GetBalanceValue()
    {
        return await this.GetLabelValue(this.BalanceLabel);
    }
    public async Task<String> GetDueDateValue()
    {
        return await this.GetLabelValue(this.DueDateLabel);
    }

    public async Task EnterPostPaymentAmount(String paymentAmount)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.PostPaymentAmountEntry);

        element.SendKeys(paymentAmount);
    }

    public async Task EnterPrePaymentAmount(String paymentAmount)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.PrePaymentAmountEntry);

        element.SendKeys(paymentAmount);
    }

    public async Task ClickMakePaymentButton()
    {
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.MakePaymentButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });
    }
}

public class TransactionsBillPaymentSuccessfulPaymentPage : BasePage2
{

    private readonly String CompleteButton;

    public TransactionsBillPaymentSuccessfulPaymentPage(TestingContext testingContext) : base(testingContext)
    {
        this.CompleteButton = "CompleteButton";
    }

    #region Properties

    protected override String Trait => "Bill Payment Successful";

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