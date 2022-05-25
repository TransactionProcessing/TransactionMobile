namespace TransactionMobile.Maui.UITests;

using System;
using System.Threading.Tasks;
using OpenQA.Selenium;

public class LoginPage : BasePage
{
    protected override String Trait => "LoginLabel";

    //private readonly String EmailEntry;
    //private readonly String PasswordEntry;
    private readonly String LoginButton;
    //private readonly String TestModeButton;
    //private readonly String ErrorLabel;

    public LoginPage()
    {
        //this.EmailEntry = "EmailEntry";
        //this.PasswordEntry = "PasswordEntry";
        this.LoginButton = "LoginButton";
        //this.TestModeButton = "TestModeButton";
        //this.ErrorLabel = "ErrorLabel";
    }

    public async Task EnterEmailAddress(String emailAddress)
    {
        //IWebElement element = await this.WaitForElementByAccessibilityId(this.EmailEntry);

        //element.SendKeys(emailAddress);
    }

    public async Task EnterPassword(String password)
    {
        //IWebElement element = await this.WaitForElementByAccessibilityId(this.PasswordEntry);
        //element.SendKeys(password);
    }

    public async Task ClickLoginButton()
    {
        this.HideKeyboard();
        IWebElement element = await this.WaitForElementByAccessibilityId(this.LoginButton);
        element.Click();
    }

    public async Task ClickTestModeButton()
    {
        //this.HideKeyboard();
        //IWebElement element = await this.WaitForElementByAccessibilityId(this.TestModeButton);
        //element.Click();
    }
}