using TransactionProcessor.Mobile.UITests.Common;
using TransactionProcessor.Mobile.UITests.Drivers;

namespace TransactionProcessor.Mobile.UITests.Pages;

public class DailyPerformanceSummaryPage : BasePage2
{
    protected override string Trait => AppiumDriverWrapper.MobileTestPlatform == MobileTestPlatform.Windows ? "Daily Performance Summary" : "DailyPerformanceSummary";

    public DailyPerformanceSummaryPage(TestingContext testingContext) : base(testingContext)
    {
    }
}
