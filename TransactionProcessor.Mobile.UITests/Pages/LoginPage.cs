using OpenQA.Selenium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class LoginPage : BasePage2
{
    protected override String Trait => "LoginLabel";

    private readonly String UserNameEntry;
    private readonly String PasswordEntry;
    private readonly String LoginButton;
    private readonly String UseTrainingModeSwitch;
    private readonly String DeviceSerial;

    private readonly String ConfigHostUrlEntry;

    public LoginPage(TestingContext testingContext) : base(testingContext)
    {
        this.UserNameEntry = "UserNameEntry";
        this.PasswordEntry = "PasswordEntry";
        this.LoginButton = "LoginButton";
        this.UseTrainingModeSwitch = "UseTrainingModeSwitch";
        this.DeviceSerial = "DeviceSerial";
        this.ConfigHostUrlEntry = "ConfigHostUrlEntry";
    }

    public async Task<String> GetDeviceSerial() {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.DeviceSerial);
        return element.Text;
    }

    public async Task SetConfigHostUrl(String configHostUrl)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.ConfigHostUrlEntry);
        element.SendKeys(configHostUrl);
    }

    public async Task<Boolean> IsTrainingModeOn()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);
        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android) {
            var text = element.GetAttribute("checked");
            if (text == "false") {
                return false;
            }

            return true;
        }
        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS) {
            String? text = element.GetAttribute("value");
            if (text == "0") {
                return false;
            }

            return true;
        }

        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows){
            return false;
        }

        return true;
    }

    public async Task SetTrainingModeOn()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);
        element.Click();
    }

    public async Task SetTrainingModeOff()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);

        element.Click();
    }

    public async Task EnterEmailAddress(String emailAddress)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UserNameEntry);

        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android){
            element.SendKeys(emailAddress);
        }
        else{
            emailAddress.ToCharArray().ToList().ForEach(x => element.SendKeys(x.ToString()));
        }
    }

    public async Task<string> GetEmailAddress()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UserNameEntry);

        return element.Text;
    }

    public async Task EnterPassword(String password)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.PasswordEntry);
        element.SendKeys(password);
    }

    public async Task ClickLoginButton(){
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.LoginButton);
                            if (element.Displayed == false)
                            {
                                this.HideKeyboard();
                            }
                            
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });

    }
}