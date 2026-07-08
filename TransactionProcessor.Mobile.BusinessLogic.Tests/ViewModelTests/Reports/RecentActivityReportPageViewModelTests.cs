using MediatR;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Reports;

public class RecentActivityReportPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogService;
    private readonly Mock<IDeviceService> DeviceService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;
    private readonly RecentActivityReportPageViewModel ViewModel;

    public RecentActivityReportPageViewModelTests()
    {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();

        this.ViewModel = new RecentActivityReportPageViewModel(this.Mediator.Object,
                                                                this.NavigationService.Object,
                                                                this.ApplicationCache.Object,
                                                                this.DialogService.Object,
                                                                this.DeviceService.Object,
                                                                this.NavigationParameterService.Object);
    }

    [Fact]
    public async Task Initialise_LoadsMockedResultsForToday()
    {
        DateTime reportDate = new(2026, 7, 6);
        this.Mediator
            .Setup(m => m.Send(It.Is<ReportQueries.GetRecentActivityReceiptReportQuery>(q => q.ReportDate == reportDate && q.SearchText == null && q.PageNumber == 1 && q.PageSize == 5), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null)));

        this.ViewModel.SelectedDate = reportDate;

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.SelectedDate.ShouldBe(reportDate);
        this.ViewModel.Items.Count.ShouldBeGreaterThan(0);
        this.ViewModel.IsLoading.ShouldBeFalse();
    }

    [Fact]
    public async Task SearchCommand_UsesSelectedDateAndSearchText()
    {
        DateTime reportDate = new(2026, 7, 6);
        this.Mediator
            .Setup(m => m.Send(It.Is<ReportQueries.GetRecentActivityReceiptReportQuery>(q => q.ReportDate == reportDate && q.SearchText == "TXN-10001" && q.PageNumber == 1 && q.PageSize == 5), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, "TXN-10001")));

        this.ViewModel.SelectedDate = reportDate;
        this.ViewModel.SearchText = "TXN-10001";

        await this.ViewModel.SearchCommand.ExecuteAsync(null);

        this.ViewModel.Items.ShouldContain(item => item.Reference == "TXN-10001");
        this.Mediator.VerifyAll();
    }

    [Fact]
    public async Task NextPageCommand_RequestsTheNextPage()
    {
        DateTime reportDate = new(2026, 7, 6);
        this.Mediator
            .Setup(m => m.Send(It.Is<ReportQueries.GetRecentActivityReceiptReportQuery>(q => q.ReportDate == reportDate && q.PageNumber == 1 && q.PageSize == 5), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null, 1, 5)));
        this.Mediator
            .Setup(m => m.Send(It.Is<ReportQueries.GetRecentActivityReceiptReportQuery>(q => q.ReportDate == reportDate && q.PageNumber == 2 && q.PageSize == 5), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null, 2, 5)));

        this.ViewModel.SelectedDate = reportDate;

        await this.ViewModel.Initialise(CancellationToken.None);
        await this.ViewModel.NextPageCommand.ExecuteAsync(null);

        this.ViewModel.PageNumber.ShouldBe(2);
        this.ViewModel.Items.ShouldAllBe(item => item.TransactionDateTime.Date == reportDate);
        this.Mediator.Verify(m => m.Send(It.Is<ReportQueries.GetRecentActivityReceiptReportQuery>(q => q.PageNumber == 2 && q.PageSize == 5), It.IsAny<CancellationToken>()), Times.Once);
    }
}
