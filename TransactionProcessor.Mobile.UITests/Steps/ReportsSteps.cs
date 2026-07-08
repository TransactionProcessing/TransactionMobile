using Reqnroll;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps;

[Binding]
[Scope(Tag = "reports")]
public class ReportsSteps{
    private readonly ReportsPage ReportsPage;
    private readonly DailyPerformanceSummaryPage DailyPerformanceSummaryPage;
    private readonly TransactionMixPage TransactionMixPage;
    private readonly RecentActivityReportPage RecentActivityReportPage;
    private readonly RecentActivityReceiptDetailPage RecentActivityReceiptDetailPage;

    public ReportsSteps(ReportsPage reportsPage,
                        DailyPerformanceSummaryPage dailyPerformanceSummaryPage,
                        TransactionMixPage transactionMixPage,
                        RecentActivityReportPage recentActivityReportPage,
                        RecentActivityReceiptDetailPage recentActivityReceiptDetailPage){
        this.ReportsPage = reportsPage;
        this.DailyPerformanceSummaryPage = dailyPerformanceSummaryPage;
        this.TransactionMixPage = transactionMixPage;
        this.RecentActivityReportPage = recentActivityReportPage;
        this.RecentActivityReceiptDetailPage = recentActivityReceiptDetailPage;
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

    [When(@"I tap on the Recent Activity and Receipt Report Button")]
    public async Task WhenITapOnTheRecentActivityAndReceiptReportButton()
    {
        await this.ReportsPage.ClickRecentActivityAndReceiptReportButton();
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

    [Then(@"the Recent Activity Report is displayed")]
    public async Task ThenTheRecentActivityReportIsDisplayed()
    {
        await this.RecentActivityReportPage.AssertOnPage();
    }

    [When(@"I tap on Search on the Recent Activity Report")]
    public async Task WhenITapOnSearchOnTheRecentActivityReport()
    {
        await this.RecentActivityReportPage.ClickSearchButton();
    }

    [When(@"I tap on the Recent Activity result for '(.*)'")]
    public async Task WhenITapOnTheRecentActivityResultFor(string reference)
    {
        await this.RecentActivityReportPage.ClickResult(reference);
    }

    [Then(@"the Recent Activity Receipt Detail Page is displayed")]
    public async Task ThenTheRecentActivityReceiptDetailPageIsDisplayed()
    {
        await this.RecentActivityReceiptDetailPage.AssertOnPage();
    }

    [Then(@"the Transaction Mix Chart is displayed")]
    public async Task ThenTheTransactionMixChartIsDisplayed()
    {
        await this.TransactionMixPage.AssertChartVisible();
    }

}
