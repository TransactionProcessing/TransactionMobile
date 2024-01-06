using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UITests;

using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Shared.IntegrationTesting;
using Shouldly;
using UiTests.Common;

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
        String? text = element.Text;
        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Android) {
            if (text == "OFF") {
                return false;
            }

            return true;
        }
        if (AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.iOS) {
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

        element.SendKeys(emailAddress);
    }

    public async Task EnterPassword(String password)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.PasswordEntry);
        element.SendKeys(password);
    }

    public async Task ClickLoginButton(){
        await Retry.For(async () => {
                            IWebElement element = await this.WaitForElementByAccessibilityId(this.LoginButton);
                            //element.Displayed.ShouldBeTrue();
                            element.Click();
                        });

    }
}