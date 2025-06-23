using TransactionMobile.Maui.Pages.Common;

namespace TransactionMobile.Maui.Pages.Reports;

using BusinessLogic.ViewModels.Reports;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using SkiaSharp;

public partial class ReportsBalanceAnalysisPage : ContentPage{
    private ReportsBalanceAnalysisPageViewModel viewModel => this.BindingContext as ReportsBalanceAnalysisPageViewModel;

    public ReportsBalanceAnalysisPage(ReportsBalanceAnalysisPageViewModel vm){
        InitializeComponent();

        this.BindingContext = vm;
    }

    protected override async void OnAppearing(){
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);

        DateTimeAxis axis1 = new DateTimeAxis(TimeSpan.FromDays(1), (d) => d.ToString("dd/MM/yyyy"));
        this.Chart.XAxes = new List<ICartesianAxis>{
                                                       axis1
                                                   };

        var axis2 = new Axis();
        axis2.Labeler = Labelers.Currency;

        this.Chart.YAxes = new List<ICartesianAxis>{
                                                       axis2
                                                   };

        this.Chart.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.CompareOnlyX;
        this.Chart.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Top;

        this.Chart.Series = new ISeries[]{
                                             new LineSeries<DateTimePoint>{
                                                                              
                                                                              Values = new List<DateTimePoint>{
                                                                                                                  new DateTimePoint(new DateTime(2024, 1, 7), 1985),
                                                                                                                  new DateTimePoint(new DateTime(2024, 1, 8), 3882),
                                                                                                                  new DateTimePoint(new DateTime(2024, 1, 11), 2511),
                                                                                                                  new DateTimePoint(new DateTime(2024, 1, 15), 3905),
                                                                                                                  new DateTimePoint(new DateTime(2024, 1, 20), 4256)
                                                                                                              },
                                                                              Fill = null,
                                                                              Name = "Balance",
                                                                              TooltipLabelFormatter =
                                                                                  (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue}"
                                                                          }
                                         };
    }

}