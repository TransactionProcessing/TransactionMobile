using CommunityToolkit.Mvvm.Input;
using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

public sealed partial class RecentActivityReportPageViewModel : ExtendedBaseViewModel
{
    private const int DefaultPageSize = 5;
    private readonly IMediator Mediator;
    private DateTime selectedDate;
    private string? searchText;
    private bool isLoading;
    private string? errorMessage;
    private RecentActivityReceiptReportModel? report;
    private int pageNumber = 1;

    public RecentActivityReportPageViewModel(IMediator mediator,
                                             INavigationService navigationService,
                                             IApplicationCache applicationCache,
                                             IDialogService dialogService,
                                             IDeviceService deviceService,
                                             INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.Title = "Recent Activity and Receipt Report";
        this.SelectedDate = DateTime.Today;
    }

    public DateTime SelectedDate
    {
        get => this.selectedDate;
        set => SetProperty(ref this.selectedDate, value);
    }

    public string? SearchText
    {
        get => this.searchText;
        set => SetProperty(ref this.searchText, value);
    }

    public bool IsLoading
    {
        get => this.isLoading;
        set => SetProperty(ref this.isLoading, value);
    }

    public string? ErrorMessage
    {
        get => this.errorMessage;
        set => SetProperty(ref this.errorMessage, value);
    }

    public RecentActivityReceiptReportModel? Report
    {
        get => this.report;
        private set => SetProperty(ref this.report, value);
    }

    public int PageNumber
    {
        get => this.pageNumber;
        private set
        {
            if (SetProperty(ref this.pageNumber, value))
            {
                this.OnPropertyChanged(nameof(this.PageDisplayText));
                this.OnPropertyChanged(nameof(this.CanGoToPreviousPage));
                this.OnPropertyChanged(nameof(this.CanGoToNextPage));
                this.OnPropertyChanged(nameof(this.HasPagination));
                this.PreviousPageCommand.NotifyCanExecuteChanged();
                this.NextPageCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public int PageSize => DefaultPageSize;

    public IReadOnlyList<RecentActivityReceiptItemModel> Items => this.Report?.Items ?? [];

    public bool HasResults => this.Items.Count > 0;

    public bool HasError => string.IsNullOrWhiteSpace(this.ErrorMessage) == false;

    public bool HasEmptyState => this.IsLoading == false && this.HasError == false && this.HasResults == false;

    public int TotalCount => this.Report?.TotalCount ?? 0;

    public int TotalPages => this.Report?.TotalPages ?? 0;

    public bool CanGoToPreviousPage => this.PageNumber > 1;

    public bool CanGoToNextPage => this.TotalPages > 0 && this.PageNumber < this.TotalPages;

    public bool HasPagination => this.TotalPages > 1;

    public string PageDisplayText => this.TotalPages <= 0
        ? string.Empty
        : $"Page {this.PageNumber} of {this.TotalPages}";

    public override async Task Initialise(CancellationToken cancellationToken)
    {
        await base.Initialise(cancellationToken);
        await this.LoadReportAsync(1, cancellationToken);
    }

    [RelayCommand]
    private async Task Search()
    {
        await this.LoadReportAsync(1, CancellationToken.None);
    }

    [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
    private async Task PreviousPage()
    {
        await this.LoadReportAsync(this.PageNumber - 1, CancellationToken.None);
    }

    [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
    private async Task NextPage()
    {
        await this.LoadReportAsync(this.PageNumber + 1, CancellationToken.None);
    }

    [RelayCommand]
    private async Task OpenItem(RecentActivityReceiptItemModel item)
    {
        await this.NavigationService.GoToRecentActivityReceiptDetailPage(item);
    }

    private async Task LoadReportAsync(int pageNumber, CancellationToken cancellationToken)
    {
        this.IsLoading = true;
        this.ErrorMessage = null;

        try
        {
            int requestedPageNumber = pageNumber > 0 ? pageNumber : 1;
            Result<RecentActivityReceiptReportModel> result = await this.Mediator.Send(new ReportQueries.GetRecentActivityReceiptReportQuery(
                                                                                              this.SelectedDate.Date,
                                                                                              string.IsNullOrWhiteSpace(this.SearchText) ? null : this.SearchText.Trim(),
                                                                                              requestedPageNumber,
                                                                                              DefaultPageSize),
                                                                                          cancellationToken);
            if (result.IsFailed)
            {
                this.Report = null;
                this.ErrorMessage = "Unable to load recent activity.";
                return;
            }

            this.Report = result.Data;
            this.PageNumber = result.Data.PageNumber > 0 ? result.Data.PageNumber : requestedPageNumber;
            this.OnPropertyChanged(nameof(this.Items));
            this.OnPropertyChanged(nameof(this.HasResults));
            this.OnPropertyChanged(nameof(this.HasEmptyState));
            this.OnPropertyChanged(nameof(this.TotalCount));
            this.OnPropertyChanged(nameof(this.TotalPages));
            this.OnPropertyChanged(nameof(this.PageDisplayText));
            this.OnPropertyChanged(nameof(this.CanGoToPreviousPage));
            this.OnPropertyChanged(nameof(this.CanGoToNextPage));
            this.OnPropertyChanged(nameof(this.HasPagination));
        }
        catch
        {
            this.Report = null;
            this.ErrorMessage = "Unable to load recent activity.";
        }
        finally
        {
            this.IsLoading = false;
            this.OnPropertyChanged(nameof(this.Items));
            this.OnPropertyChanged(nameof(this.HasResults));
            this.OnPropertyChanged(nameof(this.HasError));
            this.OnPropertyChanged(nameof(this.HasEmptyState));
            this.OnPropertyChanged(nameof(this.TotalCount));
            this.OnPropertyChanged(nameof(this.TotalPages));
            this.OnPropertyChanged(nameof(this.PageDisplayText));
            this.OnPropertyChanged(nameof(this.CanGoToPreviousPage));
            this.OnPropertyChanged(nameof(this.CanGoToNextPage));
            this.OnPropertyChanged(nameof(this.HasPagination));
        }
    }
}
