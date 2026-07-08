# Recent Activity and Receipt Report Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a dedicated recent activity and receipt report flow with a separate page stack, a single-date filter, and hardcoded results that exercise the screen flow before the real API is wired in.

**Architecture:** Add a report query and service seam that returns a mocked recent-activity payload for now, then build a new report page and a separate detail page that navigate through the existing Shell-based page stack. Keep the date input to a single `DateTime` throughout the flow so the UI, request, and mock data all match the eventual API contract.

**Tech Stack:** .NET MAUI, Shell navigation, MediatR, SimpleResults, CommunityToolkit.Mvvm, xUnit, Moq, Shouldly, MAUI UI tests.

## Global Constraints

- The report must use a single date, not a date range.
- The current implementation must return hardcoded/mock data instead of calling the API.
- The report must open as a separate page from the report landing page.
- The detail view must be a separate page reachable from the report list.
- Follow the existing report page, Shell, and viewmodel patterns already used in the app.

---

### Task 1: Add the report data contract, query, handler, and mocked service result

**Files:**
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\Models\RecentActivityReceiptReportModel.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\Requests\ReportQueries.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\RequestHandlers\ReportRequestHandler.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\Services\ReportsService.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic.Tests\RequestHandlerTests\ReportRequestHandlerTests.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic.Tests\ServicesTests\ReportsServiceTests.cs`

**Interfaces:**
- Consumes: `IApplicationCache.GetMerchantDetails()`, `IReportsService`
- Produces: `ReportQueries.GetRecentActivityReceiptReportQuery`, `IReportsService.GetRecentActivityReceiptReport(...)`, `RecentActivityReceiptReportModel.CreateMock(...)`

- [ ] **Step 1: Write the failing test**

Add one unit test that sends `new ReportQueries.GetRecentActivityReceiptReportQuery(new DateTime(2026, 7, 6), "TXN-10001")` through `ReportRequestHandler` and asserts the result contains a single-day payload with a matching reference and no date range fields.

- [ ] **Step 2: Run the test to verify it fails**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~ReportRequestHandlerTests`

Expected: compile or assertion failure because the query, handler, and model do not exist yet.

- [ ] **Step 3: Write the minimal implementation**

Add a new report model and mock factory such as:

```csharp
public sealed record RecentActivityReceiptReportModel
{
    public DateTime ReportDate { get; set; }
    public IReadOnlyList<RecentActivityReceiptItemModel> Items { get; set; } = [];
    public RecentActivityReceiptDetailModel? SelectedItem { get; set; }

    public static RecentActivityReceiptReportModel CreateMock(DateTime reportDate, string? searchTerm)
    {
        return new RecentActivityReceiptReportModel
        {
            ReportDate = reportDate.Date,
            Items =
            [
                new RecentActivityReceiptItemModel("TXN-10001", "Mobile Topup", "Safaricom", "Success", 100.00m, reportDate.Date.AddHours(9), true),
                new RecentActivityReceiptItemModel("TXN-10002", "Bill Payment", "PataPawa", "Success", 250.00m, reportDate.Date.AddHours(10), true)
            ],
            SelectedItem = new RecentActivityReceiptDetailModel("TXN-10001", "Mobile Topup", "Custom", "Safaricom", "Success", 100.00m, reportDate.Date.AddHours(9), "RCPT-10001")
        };
    }
}
```

Implement the query, handler, and service method so they pass the single date through and return the mock factory result.

- [ ] **Step 4: Run the test to verify it passes**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~ReportRequestHandlerTests`

Expected: pass.

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.BusinessLogic/Models/RecentActivityReceiptReportModel.cs TransactionProcessor.Mobile.BusinessLogic/Requests/ReportQueries.cs TransactionProcessor.Mobile.BusinessLogic/RequestHandlers/ReportRequestHandler.cs TransactionProcessor.Mobile.BusinessLogic/Services/ReportsService.cs TransactionProcessor.Mobile.BusinessLogic.Tests/RequestHandlerTests/ReportRequestHandlerTests.cs TransactionProcessor.Mobile.BusinessLogic.Tests/ServicesTests/ReportsServiceTests.cs
git commit -m "feat: add recent activity report contract"
```

### Task 2: Build the report pages and navigation flow

**Files:**
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\ViewModels\Reports\RecentActivityReportPageViewModel.cs`
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\ViewModels\Reports\RecentActivityReceiptDetailPageViewModel.cs`
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\Pages\Reports\RecentActivityReportPage.xaml`
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\Pages\Reports\RecentActivityReportPage.xaml.cs`
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\Pages\Reports\RecentActivityReceiptDetailPage.xaml`
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\Pages\Reports\RecentActivityReceiptDetailPage.xaml.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\UIServices\INavigationService.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\UIServices\ShellNavigationService.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\Extensions\MauiAppBuilderExtensions.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\AppShell.xaml`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.BusinessLogic\ViewModels\Reports\ReportsPageViewModel.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile\Pages\Reports\ReportsPage.xaml.cs`

**Interfaces:**
- Consumes: `ReportQueries.GetRecentActivityReceiptReportQuery`, `INavigationParameterService`
- Produces: `GoToRecentActivityReportPage()`, `GoToRecentActivityReceiptDetailPage(...)`, `RecentActivityReportPageViewModel`, `RecentActivityReceiptDetailPageViewModel`

- [ ] **Step 1: Write the failing test**

Add a viewmodel test that selects the new report option and verifies navigation to the report page, plus a second test that uses a single date and verifies the query carries only that one date.

- [ ] **Step 2: Run the test to verify it fails**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~RecentActivity`

Expected: compile failure because the new viewmodels, navigation methods, and page routes do not exist yet.

- [ ] **Step 3: Write the minimal implementation**

Create a report landing option for `Recent Activity and Receipt Report`, a report page with one date picker, a search box, a results list, and a tap action that opens a separate detail page. The detail page should read the selected item through `INavigationParameterService` and show the receipt/reference fields in a compact layout.

- [ ] **Step 4: Run the test to verify it passes**

Run: `dotnet test TransactionProcessor.Mobile.BusinessLogic.Tests/TransactionProcessor.Mobile.BusinessLogic.Tests.csproj --filter FullyQualifiedName~RecentActivity`

Expected: pass.

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/RecentActivityReportPageViewModel.cs TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/RecentActivityReceiptDetailPageViewModel.cs TransactionProcessor.Mobile/Pages/Reports/RecentActivityReportPage.xaml TransactionProcessor.Mobile/Pages/Reports/RecentActivityReportPage.xaml.cs TransactionProcessor.Mobile/Pages/Reports/RecentActivityReceiptDetailPage.xaml TransactionProcessor.Mobile/Pages/Reports/RecentActivityReceiptDetailPage.xaml.cs TransactionProcessor.Mobile.BusinessLogic/UIServices/INavigationService.cs TransactionProcessor.Mobile/UIServices/ShellNavigationService.cs TransactionProcessor.Mobile/Extensions/MauiAppBuilderExtensions.cs TransactionProcessor.Mobile/AppShell.xaml TransactionProcessor.Mobile.BusinessLogic/ViewModels/Reports/ReportsPageViewModel.cs TransactionProcessor.Mobile/Pages/Reports/ReportsPage.xaml.cs
git commit -m "feat: add recent activity report navigation"
```

### Task 3: Add UI test coverage for the new flow

**Files:**
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.UITests\Pages\RecentActivityReportPage.cs`
- Create: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.UITests\Pages\RecentActivityReceiptDetailPage.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.UITests\Steps\ReportsSteps.cs`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.UITests\Features\Reporting.feature`
- Modify: `D:\Projects\TransactionProcessing\TransactionMobile\TransactionProcessor.Mobile.UITests\Features\Reporting.feature.cs`

**Interfaces:**
- Consumes: the new report page automation ids and detail page automation ids
- Produces: a navigation test that reaches the report page, applies a single date, opens a result, and lands on the detail page

- [ ] **Step 1: Write the failing test**

Add a UI test scenario that navigates from Reports to the new report, selects a date, triggers search, and opens the first result.

- [ ] **Step 2: Run the test to verify it fails**

Run: `dotnet test TransactionProcessor.Mobile.UITests/TransactionProcessor.Mobile.UITests.csproj --filter FullyQualifiedName~Reporting`

Expected: failure because the new pages and automation ids are not yet wired into the UI test layer.

- [ ] **Step 3: Write the minimal implementation**

Add page objects and step definitions for the new pages, then wire the feature file to the new scenario and automation ids.

- [ ] **Step 4: Run the test to verify it passes**

Run: `dotnet test TransactionProcessor.Mobile.UITests/TransactionProcessor.Mobile.UITests.csproj --filter FullyQualifiedName~Reporting`

Expected: pass.

- [ ] **Step 5: Commit**

```bash
git add TransactionProcessor.Mobile.UITests/Pages/RecentActivityReportPage.cs TransactionProcessor.Mobile.UITests/Pages/RecentActivityReceiptDetailPage.cs TransactionProcessor.Mobile.UITests/Steps/ReportsSteps.cs TransactionProcessor.Mobile.UITests/Features/Reporting.feature TransactionProcessor.Mobile.UITests/Features/Reporting.feature.cs
git commit -m "test: cover recent activity report flow"
```
