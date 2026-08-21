using OpenQA.Selenium;
using Shared.IntegrationTesting;
using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class LoginPage : BasePage2 {
    protected override String Trait => "LoginLabel";

    private readonly String UserNameEntry;
    private readonly String PasswordEntry;
    private readonly String LoginButton;
    private readonly String UseTrainingModeSwitch;

    private readonly String ConfigHostUrlEntry;

    public LoginPage(TestingContext testingContext) : base(testingContext)
    {
        this.UserNameEntry = "UserNameEntry";
        this.PasswordEntry = "PasswordEntry";
        this.LoginButton = "LoginButton";
        this.UseTrainingModeSwitch = "UseTrainingModeSwitch";
        this.ConfigHostUrlEntry = "ConfigHostUrlEntry";
    }

    public async Task SetConfigHostUrl(String configHostUrl)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.ConfigHostUrlEntry);
        element.SendKeys(configHostUrl);
    }

    public async Task<Boolean> IsTrainingModeOn()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);
        return this.ReadTrainingModeStateOrThrow(element);
    }

    public async Task SetTrainingModeOn()
    {
        await this.SetTrainingModeState(true).ConfigureAwait(false);
    }

    public async Task SetTrainingModeOff()
    {
        await this.SetTrainingModeState(false).ConfigureAwait(false);
    }

    private async Task SetTrainingModeState(Boolean desiredState)
    {
        await Retry.For(async () =>
        {
            IWebElement element = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);
            Boolean currentState = this.ReadTrainingModeStateOrThrow(element);
            if (currentState == desiredState)
            {
                return;
            }

            element.Click();

            IWebElement refreshedElement = await this.WaitForElementByAccessibilityId(this.UseTrainingModeSwitch);
            Boolean updatedState = this.ReadTrainingModeStateOrThrow(refreshedElement);
            if (updatedState != desiredState)
            {
                throw new InvalidOperationException($"Unable to set training mode to {(desiredState ? "on" : "off")}.");
            }
        }).ConfigureAwait(false);
    }

    private Boolean ReadTrainingModeStateOrThrow(IWebElement element)
    {
        Boolean? trainingModeOn = this.TryReadTrainingModeState(element);
        if (trainingModeOn.HasValue)
        {
            return trainingModeOn.Value;
        }

        throw new InvalidOperationException("Unable to determine the training mode switch state.");
    }

    private Boolean? TryReadTrainingModeState(IWebElement element)
    {
        String?[] attributeNames =
        [
            "checked",
            "IsChecked",
            "Toggle.ToggleState",
            "ToggleState",
            "SelectionItem.IsSelected",
            "IsSelected",
            "Value.Value"
        ];

        foreach (String attributeName in attributeNames)
        {
            String? attributeValue = element.GetAttribute(attributeName);
            Boolean? parsedValue = this.TryParseSwitchState(attributeValue);
            if (parsedValue.HasValue)
            {
                return parsedValue;
            }
        }

        return null;
    }

    private Boolean? TryParseSwitchState(String? value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        String normalizedValue = new String(value.Where(Char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        return normalizedValue switch
        {
            "true" => true,
            "1" => true,
            "on" => true,
            "checked" => true,
            "selected" => true,
            "false" => false,
            "0" => false,
            "off" => false,
            "unchecked" => false,
            "unselected" => false,
            _ when normalizedValue.Contains("on", StringComparison.OrdinalIgnoreCase) && normalizedValue.Contains("off", StringComparison.OrdinalIgnoreCase) == false => true,
            _ when normalizedValue.Contains("off", StringComparison.OrdinalIgnoreCase) => false,
            _ => null,
        };
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
