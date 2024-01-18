using TransactionMobile.Maui.Pages.Common;

namespace TransactionMobile.Maui.Pages.Reports;

using BusinessLogic.ViewModels.Reports;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;

public partial class ReportsBalanceAnalysisPage : ContentPage
{
    private ReportsBalanceAnalysisPageViewModel viewModel => this.BindingContext as ReportsBalanceAnalysisPageViewModel;

    public ReportsBalanceAnalysisPage(ReportsBalanceAnalysisPageViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);

        List<ChartEntry> entries = new List<ChartEntry>();
        entries.Add(new ChartEntry(5000)
                    {
                        Label = "2024-01-01",
                        ValueLabel = "5000",
                        Color = SKColor.Parse("#2c3e50")
                    });
        entries.Add(new ChartEntry(3882)
                    {
                        Label = "2024-01-04",
                        ValueLabel = "3882",
                        Color = SKColor.Parse("#2c3e50")
                    });

        entries.Add(new ChartEntry(1985)
                    {
                        Label = "2024-01-07",
                        ValueLabel = "1985",
                        Color = SKColor.Parse("#2c3e50")
                    });
        entries.Add(new ChartEntry(3882)
                    {
                        Label = "2024-01-10",
                        ValueLabel = "3882",
                        Color = SKColor.Parse("#2c3e50")
                    });

        ChartSerie balance = new ChartSerie
                             {
                                 Entries = entries,
                                 Name = "Balance",
                                 Color = SKColor.Parse("#b455b6"),
                             };


        this.ChartView.Chart = new LineChart
                               {
                                   LabelOrientation = Orientation.Horizontal,
                                   ValueLabelOrientation = Orientation.Horizontal,
                                   LabelTextSize = 20,
                                   ValueLabelTextSize = 18,
                                   SerieLabelTextSize = 20,
                                   LegendOption = SeriesLegendOption.Bottom,
                                   Series = new List<ChartSerie> { balance }
                               };
        if (!ChartView.Chart.IsAnimating)
            await ChartView.Chart.AnimateAsync(true).ConfigureAwait(false);
    }
}