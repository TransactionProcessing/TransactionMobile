using Reqnroll;
using TransactionProcessor.Mobile.UITests.Pages;

namespace TransactionProcessor.Mobile.UITests.Steps;

[Binding]
[Scope(Tag = "reports")]
public class ReportsSteps{
    private readonly ReportsPage ReportsPage;

    private readonly SalesAnalysisPage SalesAnalysisPage;

    public ReportsSteps(ReportsPage reportsPage, SalesAnalysisPage salesAnalysisPage){
        this.ReportsPage = reportsPage;
        this.SalesAnalysisPage = salesAnalysisPage;
    }

    [Then(@"the Reports Page is displayed")]
    public async Task ThenTheReportsPageIsDisplayed()
    {
        await this.ReportsPage.AssertOnPage();
    }

    [When(@"I tap on the Sales Analysis Button")]
    public async Task WhenITapOnTheSalesAnalysisButton(){
        await this.ReportsPage.ClickSalesAnalysisButton();
    }

    [Then(@"the Sales Analysis Report is displayed")]
    public async Task ThenTheSalesAnalysisReportIsDisplayed()
    {
        await this.SalesAnalysisPage.AssertOnPage();
    }


}