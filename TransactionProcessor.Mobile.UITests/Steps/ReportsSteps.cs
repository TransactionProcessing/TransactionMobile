using Reqnroll;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps;

[Binding]
[Scope(Tag = "reports")]
public class ReportsSteps{
    private readonly ReportsPage ReportsPage;
    private readonly DailyPerformanceSummaryPage DailyPerformanceSummaryPage;
    private readonly TransactionMixPage TransactionMixPage;

    public ReportsSteps(ReportsPage reportsPage, DailyPerformanceSummaryPage dailyPerformanceSummaryPage, TransactionMixPage transactionMixPage){
        this.ReportsPage = reportsPage;
        this.DailyPerformanceSummaryPage = dailyPerformanceSummaryPage;
        this.TransactionMixPage = transactionMixPage;
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

    [When(@"I tap on the Transaction Mix Button")]
    public async Task WhenITapOnTheTransactionMixButton()
    {
        await this.ReportsPage.ClickTransactionMixButton();
    }

    [Then(@"the Daily Performance Summary Report is displayed")]
    public async Task ThenTheDailyPerformanceSummaryReportIsDisplayed()
    {
        await this.DailyPerformanceSummaryPage.AssertOnPage();
    }

    [Then(@"the Transaction Mix Report is displayed")]
    public async Task ThenTheTransactionMixReportIsDisplayed()
    {
        await this.TransactionMixPage.AssertOnPage();
        await this.TransactionMixPage.AssertSummaryVisible();
    }

    [Then(@"the Transaction Mix Chart is displayed")]
    public async Task ThenTheTransactionMixChartIsDisplayed()
    {
        await this.TransactionMixPage.AssertChartVisible();
    }

}
