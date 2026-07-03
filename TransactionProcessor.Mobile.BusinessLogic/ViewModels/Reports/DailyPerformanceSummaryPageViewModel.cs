using CommunityToolkit.Mvvm.Input;
using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

public sealed partial class DailyPerformanceSummaryPageViewModel : ExtendedBaseViewModel
{
    private readonly IMediator Mediator;
    private PerformanceSummaryPeriod selectedPeriod;
    private PerformanceSummaryPeriodOption? selectedPeriodOption;
    private bool isLoading;
    private string? errorMessage;
    private DailyPerformanceSummaryModel? summary;

    public DailyPerformanceSummaryPageViewModel(IMediator mediator,
                                                INavigationService navigationService,
                                                IApplicationCache applicationCache,
                                                IDialogService dialogService,
                                                IDeviceService deviceService,
                                                INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.Title = "Daily Performance Summary";
        this.SelectedPeriod = PerformanceSummaryPeriod.Today;
        this.PeriodOptions = Enum.GetValues<PerformanceSummaryPeriod>()
                                 .Select(period => new PerformanceSummaryPeriodOption(period, period.ToDisplayText()))
                                 .ToList();
        this.SelectedPeriodOption = this.PeriodOptions.First();
    }

    public List<PerformanceSummaryPeriodOption> PeriodOptions { get; }

    public PerformanceSummaryPeriod SelectedPeriod
    {
        get => this.selectedPeriod;
        set => SetProperty(ref this.selectedPeriod, value);
    }

    public PerformanceSummaryPeriodOption? SelectedPeriodOption
    {
        get => this.selectedPeriodOption;
        set
        {
            if (SetProperty(ref this.selectedPeriodOption, value) && value is not null)
            {
                this.SelectedPeriod = value.Period;
            }
        }
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

    public DailyPerformanceSummaryModel? Summary
    {
        get => this.summary;
        private set => SetProperty(ref this.summary, value);
    }

    public IReadOnlyList<DailyPerformanceMetricModel> SummaryCards => this.Summary?.Metrics ?? new List<DailyPerformanceMetricModel>();

    public IReadOnlyList<DailyPerformanceMetricModel> TopSummaryCards => this.SummaryCards.Take(4).ToList();

    public IReadOnlyList<DailyPerformanceMetricModel> TopSummaryCardsRow1 => this.TopSummaryCards.Take(2).ToList();

    public IReadOnlyList<DailyPerformanceMetricModel> TopSummaryCardsRow2 => this.TopSummaryCards.Skip(2).Take(2).ToList();

    public IReadOnlyList<DailyPerformanceTransactionModel> DrillDownTransactions => this.Summary?.DrillDownTransactions ?? new List<DailyPerformanceTransactionModel>();

    public bool HasSummaryData => this.SummaryCards.Count > 0;

    public bool HasError => string.IsNullOrWhiteSpace(this.ErrorMessage) == false;

    public bool HasEmptyState => this.IsLoading == false && this.HasError == false && this.HasSummaryData == false;

    public override async Task Initialise(CancellationToken cancellationToken)
    {
        await base.Initialise(cancellationToken);
        await this.LoadSummaryAsync(this.SelectedPeriod, cancellationToken);
    }

    [RelayCommand]
    private async Task PeriodChanged()
    {
        if (this.SelectedPeriodOption is not null)
        {
            this.SelectedPeriod = this.SelectedPeriodOption.Period;
        }

        await this.LoadSummaryAsync(this.SelectedPeriod, CancellationToken.None);
    }

    private async Task LoadSummaryAsync(PerformanceSummaryPeriod period, CancellationToken cancellationToken)
    {
        this.IsLoading = true;
        this.ErrorMessage = null;

        try
        {
            Result<DailyPerformanceSummaryModel> result = await this.Mediator.Send(new ReportQueries.GetDailyPerformanceSummaryQuery(period), cancellationToken);
            if (result.IsFailed)
            {
                this.Summary = null;
                this.ErrorMessage = "Unable to load performance summary.";
                return;
            }

            this.Summary = result.Data;
        }
        catch(Exception ex)
        {
            this.Summary = null;
            this.ErrorMessage = "Unable to load performance summary.";
        }
        finally
        {
            this.IsLoading = false;
            this.OnPropertyChanged(nameof(this.SummaryCards));
            this.OnPropertyChanged(nameof(this.TopSummaryCards));
            this.OnPropertyChanged(nameof(this.TopSummaryCardsRow1));
            this.OnPropertyChanged(nameof(this.TopSummaryCardsRow2));
            this.OnPropertyChanged(nameof(this.DrillDownTransactions));
            this.OnPropertyChanged(nameof(this.HasSummaryData));
            this.OnPropertyChanged(nameof(this.HasError));
            this.OnPropertyChanged(nameof(this.HasEmptyState));
        }
    }
}

public sealed record PerformanceSummaryPeriodOption(PerformanceSummaryPeriod Period, string DisplayText);

internal static class PerformanceSummaryPeriodExtensions
{
    public static string ToDisplayText(this PerformanceSummaryPeriod period) => period switch
    {
        PerformanceSummaryPeriod.Today => "Today",
        PerformanceSummaryPeriod.Yesterday => "Yesterday",
        PerformanceSummaryPeriod.ThisWeek => "This Week",
        PerformanceSummaryPeriod.MonthToDate => "Month to Date",
        _ => period.ToString(),
    };
}
