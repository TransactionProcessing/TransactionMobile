using MediatR;
using Imposter.Abstractions;
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
    private readonly IMediatorImposter Mediator;
    private readonly INavigationServiceImposter NavigationService;
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly IDialogServiceImposter DialogService;
    private readonly IDeviceServiceImposter DeviceService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;
    private readonly RecentActivityReportPageViewModel ViewModel;

    public RecentActivityReportPageViewModelTests()
    {
        this.Mediator = new IMediatorImposter();
        this.NavigationService = new INavigationServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();

        this.ViewModel = new RecentActivityReportPageViewModel(this.Mediator.Instance(),
                                                                this.NavigationService.Instance(),
                                                                this.ApplicationCache.Instance(),
                                                                this.DialogService.Instance(),
                                                                this.DeviceService.Instance(),
                                                                this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task Initialise_LoadsMockedResultsForToday()
    {
        DateTime reportDate = new(2026, 7, 6);
        this.Mediator
            .Send(Arg<IRequest<Result<RecentActivityReceiptReportModel>>>.Is(q => ((ReportQueries.GetRecentActivityReceiptReportQuery)q).ReportDate == reportDate && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).SearchText == null && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageNumber == 1 && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageSize == 5), Arg<CancellationToken>.Any())
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
            .Send(Arg<IRequest<Result<RecentActivityReceiptReportModel>>>.Is(q => ((ReportQueries.GetRecentActivityReceiptReportQuery)q).ReportDate == reportDate && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).SearchText == "TXN-10001" && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageNumber == 1 && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageSize == 5), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, "TXN-10001")));

        this.ViewModel.SelectedDate = reportDate;
        this.ViewModel.SearchText = "TXN-10001";

        await this.ViewModel.SearchCommand.ExecuteAsync(null);

        this.ViewModel.Items.ShouldContain(item => item.Reference == "TXN-10001");
        this.Mediator.Send(Arg<IRequest<Result<RecentActivityReceiptReportModel>>>.Is(q => ((ReportQueries.GetRecentActivityReceiptReportQuery)q).ReportDate == reportDate && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).SearchText == "TXN-10001" && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageNumber == 1 && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageSize == 5), Arg<CancellationToken>.Any())
            .Called(Count.Once());
    }

    [Fact]
    public async Task NextPageCommand_RequestsTheNextPage()
    {
        DateTime reportDate = new(2026, 7, 6);
        this.Mediator
            .Send(Arg<IRequest<Result<RecentActivityReceiptReportModel>>>.Is(q => ((ReportQueries.GetRecentActivityReceiptReportQuery)q).ReportDate == reportDate && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageNumber == 1 && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageSize == 5), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null, 1, 5)));
        this.Mediator
            .Send(Arg<IRequest<Result<RecentActivityReceiptReportModel>>>.Is(q => ((ReportQueries.GetRecentActivityReceiptReportQuery)q).ReportDate == reportDate && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageNumber == 2 && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageSize == 5), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(RecentActivityReceiptReportModel.CreateMock(reportDate, null, 2, 5)));

        this.ViewModel.SelectedDate = reportDate;

        await this.ViewModel.Initialise(CancellationToken.None);
        await this.ViewModel.NextPageCommand.ExecuteAsync(null);

        this.ViewModel.PageNumber.ShouldBe(2);
        this.ViewModel.Items.ShouldAllBe(item => item.TransactionDateTime.Date == reportDate);
        this.Mediator.Send(Arg<IRequest<Result<RecentActivityReceiptReportModel>>>.Is(q => ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageNumber == 2 && ((ReportQueries.GetRecentActivityReceiptReportQuery)q).PageSize == 5), Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
