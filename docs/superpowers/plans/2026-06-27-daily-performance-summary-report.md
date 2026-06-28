# Daily Performance Summary Report Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a merchant-facing daily performance summary report backed by a MediatR query that initially returns mocked test data.

**Architecture:** Add a new report query/handler pair in the business layer, backed by a small report summary model and drill-down item model. Expose the data through a new mobile report page and view model, then wire it into the existing Reports hub and navigation service. Keep the mock data isolated in the handler so it can later be replaced with a real backend API call without changing the screen.

**Tech Stack:** .NET MAUI, C#, MediatR, CommunityToolkit.Mvvm, existing MAUI Shell navigation, existing test projects.

---

### Task 1: Add report contract and mocked handler

**Files:**
- Create: `TransactionProcessor.Mobile.BusinessLogic/Requests/ReportQueries.cs`
- Create: `TransactionProcessor.Mobile.BusinessLogic/RequestHandlers/ReportRequestHandler.cs`
- Test: `TransactionProcessor.Mobile.BusinessLogic.Tests/RequestHandlerTests/ReportRequestHandlerTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
[Fact]
public async Task GetDailyPerformanceSummaryQuery_ReturnsSummaryForToday()
{
    Result<DailyPerformanceSummaryModel> result = await this.Mediator.Send(new ReportQueries.GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod.Today), CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Data.Period.ShouldBe(PerformanceSummaryPeriod.Today);
    result.Data.Metrics.ShouldContain(m => m.Title == "Total transaction count" && m.Value == "48");
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~ReportRequestHandlerTests -v minimal`
Expected: fail because the query/handler/model do not exist yet.

- [ ] **Step 3: Write minimal implementation**

```csharp
public record ReportQueries
{
    public record GetDailyPerformanceSummaryQuery(PerformanceSummaryPeriod Period) : IRequest<Result<DailyPerformanceSummaryModel>>;
}
```

```csharp
public class ReportRequestHandler : IRequestHandler<ReportQueries.GetDailyPerformanceSummaryQuery, Result<DailyPerformanceSummaryModel>>
{
    public Task<Result<DailyPerformanceSummaryModel>> Handle(ReportQueries.GetDailyPerformanceSummaryQuery request, CancellationToken cancellationToken)
    {
        DailyPerformanceSummaryModel model = DailyPerformanceSummaryModel.CreateMock(request.Period);
        return Task.FromResult(Result.Success(model));
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~ReportRequestHandlerTests -v minimal`
Expected: pass.

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.BusinessLogic/Requests/ReportQueries.cs TransactionProcessor.Mobile.BusinessLogic/RequestHandlers/ReportRequestHandler.cs TransactionProcessor.Mobile.BusinessLogic.Tests/RequestHandlerTests/ReportRequestHandlerTests.cs
git commit -m "feat: add mocked daily report query"
```

### Task 2: Add report view model and mobile page

**Files:**
- Create: `TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/DailyPerformanceSummaryPageViewModel.cs`
- Create: `TransactionProcessor.Mobile/Pages/Reports/DailyPerformanceSummaryPage.xaml`
- Create: `TransactionProcessor.Mobile/Pages/Reports/DailyPerformanceSummaryPage.xaml.cs`
- Test: `TransactionProcessor.Mobile.BusinessLogic.Tests/ViewModelTests/Reports/DailyPerformanceSummaryPageViewModelTests.cs`

- [ ] **Step 1: Write the failing test**

```csharp
[Fact]
public async Task Initialise_LoadsSummaryCards()
{
    await this.ViewModel.Initialise(CancellationToken.None);

    this.ViewModel.SummaryCards.Count.ShouldBeGreaterThan(0);
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~DailyPerformanceSummaryPageViewModelTests -v minimal`
Expected: fail because the view model does not exist yet.

- [ ] **Step 3: Write minimal implementation**

```csharp
public partial class DailyPerformanceSummaryPageViewModel : ExtendedBaseViewModel
{
    public override async Task Initialise(CancellationToken cancellationToken)
    {
        Result<DailyPerformanceSummaryModel> result = await this.Mediator.Send(new ReportQueries.GetDailyPerformanceSummaryQuery(this.SelectedPeriod), cancellationToken);
        this.SummaryCards = result.Data.Metrics;
    }
}
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~DailyPerformanceSummaryPageViewModelTests -v minimal`
Expected: pass.

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/DailyPerformanceSummaryPageViewModel.cs TransactionProcessor.Mobile/Pages/Reports/DailyPerformanceSummaryPage.xaml TransactionProcessor.Mobile/Pages/Reports/DailyPerformanceSummaryPage.xaml.cs TransactionProcessor.Mobile.BusinessLogic.Tests/ViewModelTests/Reports/DailyPerformanceSummaryPageViewModelTests.cs
git commit -m "feat: add daily performance report screen"
```

### Task 3: Wire navigation and reports hub

**Files:**
- Modify: `TransactionProcessor.Mobile.BusinessLogic/UIServices/INavigationService.cs`
- Modify: `TransactionProcessor.Mobile/UIServices/ShellNavigationService.cs`
- Modify: `TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/ReportsPageViewModel.cs`
- Modify: `TransactionProcessor.Mobile/Pages/Reports/ReportsPage.xaml`
- Modify: `TransactionProcessor.Mobile/App.xaml.cs`
- Modify: `TransactionProcessor.Mobile.Extensions/MauiAppBuilderExtensions.cs`

- [ ] **Step 1: Write the failing test**

```csharp
[Fact]
public async Task ReportsPage_Initialise_AddsDailySummaryOption()
{
    await this.ViewModel.Initialise(CancellationToken.None);

    this.ViewModel.ReportsMenuOptions.ShouldContain(x => x.Title == "Daily Performance Summary");
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~ReportsPageViewModelTests -v minimal`
Expected: fail until the new option and navigation route exist.

- [ ] **Step 3: Write minimal implementation**

```csharp
public Task GoToDailyPerformanceSummaryPage();
```

```csharp
this.ReportsMenuOptions = new List<ListViewItem> {
    new ListViewItem { Title = "Daily Performance Summary" },
    new ListViewItem { Title = "Sales Analysis" },
    new ListViewItem { Title = "Balance Analysis" }
};
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~ReportsPageViewModelTests -v minimal`
Expected: pass.

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.BusinessLogic/UIServices/INavigationService.cs TransactionProcessor.Mobile/UIServices/ShellNavigationService.cs TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/ReportsPageViewModel.cs TransactionProcessor.Mobile/Pages/Reports/ReportsPage.xaml TransactionProcessor.Mobile/App.xaml.cs TransactionProcessor.Mobile.Extensions/MauiAppBuilderExtensions.cs
git commit -m "feat: wire daily performance summary navigation"
```

### Task 4: Add UI and navigation tests

**Files:**
- Modify: `TransactionProcessor.Mobile.UITests/Pages/ReportsPage.cs`
- Modify: `TransactionProcessor.Mobile.UITests/Steps/ReportsSteps.cs`
- Modify: `TransactionProcessor.Mobile.UITests/Features/PageNavigation.feature`
- Modify: `TransactionProcessor.Mobile.UITests/Features/HardwarePageNavigation.feature`

- [ ] **Step 1: Write the failing test**

```gherkin
Then the Daily Performance Summary Report is displayed
```

- [ ] **Step 2: Run test to verify it fails**

Run the relevant UI test project command already used in this repo.

- [ ] **Step 3: Write minimal implementation**

```csharp
public async Task ClickDailyPerformanceSummaryButton()
{
    IWebElement element = await this.WaitForElementByAccessibilityId(this.DailyPerformanceSummaryButton);
    element.Click();
}
```

- [ ] **Step 4: Run test to verify it passes**

Run the updated UI scenario(s).

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.UITests/Pages/ReportsPage.cs TransactionProcessor.Mobile.UITests/Steps/ReportsSteps.cs TransactionProcessor.Mobile.UITests/Features/PageNavigation.feature TransactionProcessor.Mobile.UITests/Features/HardwarePageNavigation.feature
git commit -m "test: cover daily performance report navigation"
```
