using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class TransactionMixPage : BasePage2
{
    protected override string Trait => AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows ? "Transaction Mix" : "TransactionMix";

    private readonly string SummarySection;

    public TransactionMixPage(TestingContext testingContext) : base(testingContext)
    {
        this.SummarySection = "TransactionMixSummary";
    }

    public async Task AssertSummaryVisible()
    {
        await this.WaitForElementByAccessibilityId(this.SummarySection);
    }

    public async Task AssertChartVisible()
    {
        await this.WaitForElementByAccessibilityId("TransactionMixChart");
    }
}
