using OpenQA.Selenium;
using TransactionProcessor.Mobile.UITests.Common;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class RecentActivityReportPage : BasePage2
{
    protected override string Trait => "RecentActivityReport";
    private const string ResultAutomationIdPrefix = "RecentActivityResult";

    public RecentActivityReportPage(TestingContext testingContext) : base(testingContext)
    {
    }

    public async Task EnterSearchText(string searchText)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId("RecentActivitySearchText");
        element.Clear();
        element.SendKeys(searchText);
    }

    public async Task ClickSearchButton()
    {
        IWebElement element = await this.WaitForElementByAccessibilityId("RecentActivitySearchButton");
        element.Click();
    }

    public async Task ClickResult(string reference)
    {
        IWebElement element = await this.WaitForElementByAccessibilityId(BuildAutomationId(reference));
        element.Click();
    }

    private static string BuildAutomationId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ResultAutomationIdPrefix;
        }

        var builder = new System.Text.StringBuilder(ResultAutomationIdPrefix.Length + value.Length + 1);
        builder.Append(ResultAutomationIdPrefix);
        builder.Append('_');

        bool appendedCharacter = false;
        bool lastWasSeparator = false;

        foreach (char character in value.Trim())
        {
            char sanitized = char.IsLetterOrDigit(character) ? character : '_';

            if (sanitized == '_')
            {
                if (lastWasSeparator)
                {
                    continue;
                }

                lastWasSeparator = true;
            }
            else
            {
                lastWasSeparator = false;
            }

            builder.Append(sanitized);
            appendedCharacter = true;
        }

        if (appendedCharacter == false)
        {
            return ResultAutomationIdPrefix;
        }

        if (builder.Length > 0 && builder[^1] == '_')
        {
            builder.Length--;
        }

        return builder.ToString();
    }
}
