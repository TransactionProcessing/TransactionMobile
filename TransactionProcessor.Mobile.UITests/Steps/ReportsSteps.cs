using Reqnroll;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps;

[Binding]
[Scope(Tag = "reports")]
public class ReportsSteps{
    private readonly ReportsPage ReportsPage;
    private readonly DailyPerformanceSummaryPage DailyPerformanceSummaryPage;

    public ReportsSteps(ReportsPage reportsPage, DailyPerformanceSummaryPage dailyPerformanceSummaryPage){
        this.ReportsPage = reportsPage;
        this.DailyPerformanceSummaryPage = dailyPerformanceSummaryPage;
    }

    [Then(@"the Reports Page is displayed")]
    public async Task ThenTheReportsPageIsDisplayed()
    {
        await this.ReportsPage.AssertOnPage();
    }

    [When(@"I tap on the Daily Performance Summary Button")]
    public async Task WhenITapOnTheDailyPerformanceSummaryButton()
    {
        await this.ReportsPage.ClickDailyPerformanceSummaryButton();
    }

    [Then(@"the Daily Performance Summary Report is displayed")]
    public async Task ThenTheDailyPerformanceSummaryReportIsDisplayed()
    {
        await this.DailyPerformanceSummaryPage.AssertOnPage();
    }

}
