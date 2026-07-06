using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

public sealed partial class TransactionMixPageViewModel : ExtendedBaseViewModel
{
    private readonly IMediator Mediator;
    private TransactionMixBreakdown selectedBreakdown;
    private TransactionMixMeasure selectedMeasure;
    private DateTime startDate;
    private DateTime endDate;
    private bool isLoading;
    private string? errorMessage;
    private TransactionMixSummaryModel? summary;
    private TransactionMixSummaryItemModel? selectedItem;
    private int topN;
    private TransactionMixBreakdownOption? selectedBreakdownOption;
    private TransactionMixMeasureOption? selectedMeasureOption;
    private ISeries[] chartSeries = [];
    private List<ICartesianAxis> chartXAxes = [];
    private List<ICartesianAxis> chartYAxes = [];

    public TransactionMixPageViewModel(IMediator mediator,
                                       INavigationService navigationService,
                                       IApplicationCache applicationCache,
                                       IDialogService dialogService,
                                       IDeviceService deviceService,
                                       INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.Title = "Transaction Mix";
        this.SelectedBreakdown = TransactionMixBreakdown.TransactionType;
        this.SelectedMeasure = TransactionMixMeasure.Count;
        this.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        this.EndDate = DateTime.Today;
        this.TopN = 5;
        this.BreakdownOptions = Enum.GetValues<TransactionMixBreakdown>()
                                    .Where(option => option != TransactionMixBreakdown.NotSet)
                                    .Select(option => new TransactionMixBreakdownOption(option, option.ToDisplayText()))
                                    .ToList();
        this.MeasureOptions = Enum.GetValues<TransactionMixMeasure>()
                                  .Where(option => option != TransactionMixMeasure.NotSet)
                                  .Select(option => new TransactionMixMeasureOption(option, option.ToDisplayText()))
                                  .ToList();
        this.SelectedBreakdownOption = this.BreakdownOptions.First();
        this.SelectedMeasureOption = this.MeasureOptions.First();
    }

    public List<TransactionMixBreakdownOption> BreakdownOptions { get; }

    public List<TransactionMixMeasureOption> MeasureOptions { get; }

    public TransactionMixBreakdown SelectedBreakdown
    {
        get => this.selectedBreakdown;
        set => SetProperty(ref this.selectedBreakdown, value);
    }

    public TransactionMixBreakdownOption? SelectedBreakdownOption
    {
        get => this.selectedBreakdownOption;
        set
        {
            if (SetProperty(ref this.selectedBreakdownOption, value) && value is not null)
            {
                this.SelectedBreakdown = value.Breakdown;
            }
        }
    }

    public TransactionMixMeasure SelectedMeasure
    {
        get => this.selectedMeasure;
        set => SetProperty(ref this.selectedMeasure, value);
    }

    public TransactionMixMeasureOption? SelectedMeasureOption
    {
        get => this.selectedMeasureOption;
        set
        {
            if (SetProperty(ref this.selectedMeasureOption, value) && value is not null)
            {
                this.SelectedMeasure = value.Measure;
            }
        }
    }

    public DateTime StartDate
    {
        get => this.startDate;
        set => SetProperty(ref this.startDate, value);
    }

    public DateTime EndDate
    {
        get => this.endDate;
        set => SetProperty(ref this.endDate, value);
    }

    public int TopN
    {
        get => this.topN;
        set => SetProperty(ref this.topN, value);
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

    public TransactionMixSummaryModel? Summary
    {
        get => this.summary;
        private set => SetProperty(ref this.summary, value);
    }

    public TransactionMixSummaryItemModel? SelectedItem
    {
        get => this.selectedItem;
        set
        {
            if (SetProperty(ref this.selectedItem, value))
            {
                this.OnPropertyChanged(nameof(this.SelectedItemTransactions));
                this.OnPropertyChanged(nameof(this.SelectedItemLabel));
            }
        }
    }

    public IReadOnlyList<TransactionMixSummaryItemModel> Items => this.Summary?.Items ?? [];

    public IReadOnlyList<TransactionMixSummaryItemModel> TopItems => this.Items.Take(this.TopN).ToList();

    public IReadOnlyList<TransactionMixDrillDownTransactionModel> DrillDownTransactions => this.Summary?.DrillDownTransactions ?? [];

    public ISeries[] ChartSeries
    {
        get => this.chartSeries;
        private set => SetProperty(ref this.chartSeries, value);
    }

    public List<ICartesianAxis> ChartXAxes
    {
        get => this.chartXAxes;
        private set => SetProperty(ref this.chartXAxes, value);
    }

    public List<ICartesianAxis> ChartYAxes
    {
        get => this.chartYAxes;
        private set => SetProperty(ref this.chartYAxes, value);
    }

    public bool HasChartData => this.ChartSeries.Length > 0;

    public string ChartSubtitle => this.HasChartData
        ? $"Top {this.TopItems.Count} {this.SelectedBreakdown.ToDisplayText()} categories by {this.SelectedMeasure.ToDisplayText().ToLowerInvariant()}"
        : "No chart data available";

    public IReadOnlyList<TransactionMixDrillDownTransactionModel> SelectedItemTransactions
    {
        get
        {
            if (this.Summary is null)
            {
                return [];
            }

            return this.DrillDownTransactions
                       .Where(this.MatchesSelectedItem)
                       .Take(this.TopN)
                       .ToList();
        }
    }

    public string SelectedItemLabel => this.SelectedItem?.Label ?? "All transactions";

    public bool HasSummaryData => this.Items.Count > 0;

    public bool HasError => string.IsNullOrWhiteSpace(this.ErrorMessage) == false;

    public bool HasEmptyState => this.IsLoading == false && this.HasError == false && this.HasSummaryData == false;

    public override async Task Initialise(CancellationToken cancellationToken)
    {
        await base.Initialise(cancellationToken);
        await this.LoadSummaryAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task ApplyFilters()
    {
        await this.LoadSummaryAsync(CancellationToken.None);
    }

    [RelayCommand]
    private void SelectItem(TransactionMixSummaryItemModel item)
    {
        this.SelectedItem = item;
        this.OnPropertyChanged(nameof(this.SelectedItemTransactions));
    }

    private async Task LoadSummaryAsync(CancellationToken cancellationToken)
    {
        this.IsLoading = true;
        this.ErrorMessage = null;

        try
        {
            Result<TransactionMixSummaryModel> result = await this.Mediator.Send(new ReportQueries.GetTransactionMixSummaryQuery(
                                                                                   this.StartDate.Date,
                                                                                   this.EndDate.Date,
                                                                                   this.SelectedBreakdown,
                                                                                   this.SelectedMeasure,
                                                                                   this.TopN),
                                                                               cancellationToken);
            if (result.IsFailed)
            {
                this.Summary = null;
                this.SelectedItem = null;
                this.ChartSeries = [];
                this.ChartXAxes = [];
                this.ChartYAxes = [];
                this.ErrorMessage = "Unable to load transaction mix.";
                return;
            }

            this.Summary = result.Data;
            this.SelectedItem = null;
            this.UpdateChart();
            this.OnPropertyChanged(nameof(this.Items));
            this.OnPropertyChanged(nameof(this.TopItems));
            this.OnPropertyChanged(nameof(this.DrillDownTransactions));
            this.OnPropertyChanged(nameof(this.SelectedItemTransactions));
            this.OnPropertyChanged(nameof(this.HasChartData));
            this.OnPropertyChanged(nameof(this.HasSummaryData));
            this.OnPropertyChanged(nameof(this.HasEmptyState));
        }
        catch
        {
            this.Summary = null;
            this.SelectedItem = null;
            this.ChartSeries = [];
            this.ChartXAxes = [];
            this.ChartYAxes = [];
            this.ErrorMessage = "Unable to load transaction mix.";
        }
        finally
        {
            this.IsLoading = false;
            this.OnPropertyChanged(nameof(this.Items));
            this.OnPropertyChanged(nameof(this.TopItems));
            this.OnPropertyChanged(nameof(this.DrillDownTransactions));
            this.OnPropertyChanged(nameof(this.SelectedItemTransactions));
            this.OnPropertyChanged(nameof(this.SelectedItemLabel));
            this.OnPropertyChanged(nameof(this.HasChartData));
            this.OnPropertyChanged(nameof(this.HasSummaryData));
            this.OnPropertyChanged(nameof(this.HasError));
            this.OnPropertyChanged(nameof(this.HasEmptyState));
        }
    }

    private bool MatchesSelectedItem(TransactionMixDrillDownTransactionModel transaction)
    {
        if (this.SelectedItem is null)
        {
            return true;
        }

        return this.SelectedBreakdown switch
        {
            TransactionMixBreakdown.TransactionType => string.Equals(transaction.TransactionType, this.SelectedItem.Label, StringComparison.OrdinalIgnoreCase),
            TransactionMixBreakdown.Product => string.Equals(transaction.Product, this.SelectedItem.Label, StringComparison.OrdinalIgnoreCase),
            TransactionMixBreakdown.Operator => string.Equals(transaction.Operator, this.SelectedItem.Label, StringComparison.OrdinalIgnoreCase),
            TransactionMixBreakdown.Status => string.Equals(transaction.Status, this.SelectedItem.Label, StringComparison.OrdinalIgnoreCase),
            _ => true,
        };
    }

    private void UpdateChart()
    {
        IReadOnlyList<TransactionMixSummaryItemModel> topItems = this.TopItems;
        if (topItems.Count == 0)
        {
            this.ChartSeries = [];
            this.ChartXAxes = [];
            this.ChartYAxes = [];
            return;
        }

        List<double> values = topItems.Select(item => this.SelectedMeasure == TransactionMixMeasure.Value ? (double)item.Value : (double)item.Count).ToList();

        this.ChartSeries = topItems.Select((item, index) =>
        {
            double value = values[index];
            return new PieSeries<double>(new[] { value })
            {
                Name = item.Label,
            };
        }).Cast<ISeries>().ToArray();

        this.ChartYAxes = [];
        this.ChartXAxes = [];

        this.OnPropertyChanged(nameof(this.ChartSeries));
        this.OnPropertyChanged(nameof(this.ChartXAxes));
        this.OnPropertyChanged(nameof(this.ChartYAxes));
        this.OnPropertyChanged(nameof(this.HasChartData));
        this.OnPropertyChanged(nameof(this.ChartSubtitle));
    }
}

public sealed record TransactionMixBreakdownOption(TransactionMixBreakdown Breakdown, string DisplayText);

public sealed record TransactionMixMeasureOption(TransactionMixMeasure Measure, string DisplayText);

internal static class TransactionMixDisplayExtensions
{
    public static string ToDisplayText(this TransactionMixBreakdown breakdown) => breakdown switch
    {
        TransactionMixBreakdown.TransactionType => "Transaction Type",
        TransactionMixBreakdown.Product => "Product",
        TransactionMixBreakdown.Operator => "Operator",
        TransactionMixBreakdown.Status => "Status",
        _ => breakdown.ToString(),
    };

    public static string ToDisplayText(this TransactionMixMeasure measure) => measure switch
    {
        TransactionMixMeasure.Count => "Count",
        TransactionMixMeasure.Value => "Value",
        _ => measure.ToString(),
    };
}
