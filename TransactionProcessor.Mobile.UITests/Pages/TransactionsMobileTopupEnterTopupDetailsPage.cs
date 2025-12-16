using OpenQA.Selenium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionsMobileTopupEnterTopupDetailsPage : BasePage2 {

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

    protected override String Trait => "EnterTopupDetails";

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