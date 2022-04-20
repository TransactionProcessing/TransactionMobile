namespace TransactionMobile.Maui.UITests;

using System;
using System.Threading.Tasks;

public class MainPage : BasePage
{
    protected override String Trait => "Home";

    private readonly String TransactionsButton;

    private readonly String ReportsButton;

    private readonly String ProfileButton;

    private readonly String SupportButton;

    private readonly String AvailableBalanceLabel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage()
    {
        this.TransactionsButton = "TransactionsButton";
        this.ReportsButton = "ReportsButton";
        this.ProfileButton = "ProfileButton";
        this.SupportButton = "SupportButton";
        this.AvailableBalanceLabel = "AvailableBalanceValueLabel";
    }

    public async Task ClickTransactionsButton()
    {
        var element = await this.WaitForElementByAccessibilityId(this.TransactionsButton);
        element.Click();
    }

    public async Task ClickReportsButton()
    {
        var element = await this.WaitForElementByAccessibilityId(this.ReportsButton);
        element.Click();
    }

    public void ClickProfileButton()
    {
        //app.WaitForElement(this.ProfileButton);
        //app.Tap(this.ProfileButton);
    }

    public void ClickSupportButton()
    {
        //app.WaitForElement(this.SupportButton);
        //app.Tap(this.SupportButton);
    }

    public async Task<Decimal> GetAvailableBalanceValue(TimeSpan? timeout = default(TimeSpan?))
    {
        //await this.ScrollTo(this.Trait, this.AvailableBalanceLabel);
        var element = await this.WaitForElementByAccessibilityId(this.AvailableBalanceLabel, timeout: TimeSpan.FromSeconds(30));

        String availableBalanceText = element.Text.Replace(" KES", String.Empty);

        if (Decimal.TryParse(availableBalanceText, out Decimal balanceValue) == false)
        {
            throw new Exception($"Failed to parse [{availableBalanceText}] as a Decimal");
        }

        return balanceValue;
    }
}