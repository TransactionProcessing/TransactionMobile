using TransactionMobile.Maui.UiTests.Drivers;

namespace TransactionMobile.Maui.UITests;

using System;
using System.Threading.Tasks;
using OpenQA.Selenium;

public class LoginPage : BasePage
{
    protected override String Trait => "LoginLabel";

    private readonly String UserNameEntry;
    private readonly String PasswordEntry;
    private readonly String LoginButton;
    private readonly String UseTrainingModeSwitch;
    //private readonly String TestModeButton;
    //private readonly String ErrorLabel;

    public LoginPage()
    {
        this.UserNameEntry = "UserNameEntry";
        this.PasswordEntry = "PasswordEntry";
        this.LoginButton = "LoginButton";
        this.UseTrainingModeSwitch = "UseTrainingModeSwitch";
        //this.TestModeButton = "TestModeButton";
        //this.ErrorLabel = "ErrorLabel";
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

        return true;
    }

    public async Task SetTrainingModeOn()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);
        var text = element.Text;
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

    public async Task ClickLoginButton()
    {
        //this.HideKeyboard();
        IWebElement element = await this.WaitForElementByAccessibilityId(this.LoginButton);
        element.Click();
    }
}