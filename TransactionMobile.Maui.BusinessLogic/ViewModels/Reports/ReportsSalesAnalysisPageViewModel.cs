namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Reports;

using Maui.UIServices;
using MediatR;
using Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MvvmHelpers.Commands;
using UIServices;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class ReportsSalesAnalysisPageViewModel : ExtendedBaseViewModel{

    private readonly IMediator Mediator;
    public ReportsSalesAnalysisPageViewModel(INavigationService navigationService,
                                             IApplicationCache applicationCache,
                                             IDialogService dialogService,
                                             IDeviceService deviceService,
                                             IMediator mediator, INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService,navigationParameterService)
    {
        this.Mediator = mediator;
        //this.ComparisonDatePickerSelectedIndexChangedCommand = new AsyncCommand(this.ComparisonDatePickerSelectedExecute);
        this.Title = "Sales Analysis";
    }

    //public ICommand ComparisonDatePickerSelectedIndexChangedCommand { get; }

    //private async Task ComparisonDatePickerSelectedExecute(){
    //    // TODO: we will re fire the api calls here on a date change
    //    await this.GetApiData();
    //}

    [RelayCommand]
    private async Task ComparisonDatePickerSelectedIndexChanged()
    {
        //// Access the updated SelectedItem property directly here
        //if (_selectedItem != null)
        //{
        //    Console.WriteLine($"Picker selection changed to: {_selectedItem.DisplayText}");
        //    // Perform your logic
        //}
        await this.GetApiData();
    }

    private async Task GetApiData(){
        // TODO: Initial api call to get data would be done here
        List<SalesAnalysis> salesAnalysisList = new List<SalesAnalysis>();
        salesAnalysisList.Add(new SalesAnalysis("100.00 KES", "90.00 KES", "10%", "Sales Value", "Today's Sales", SelectedItem.DisplayText, "Variance:","salesvalue.png"));
        salesAnalysisList.Add(new SalesAnalysis("100", "90", "10%", "Sales Count", "Today's Sales", SelectedItem.DisplayText, "Variance:", "salescount.png"));
        this.SalesAnalysisList = salesAnalysisList;
    }

    
    private List<ComparisonDate> comparisonDates;

    ComparisonDate _selectedItem;

    private List<SalesAnalysis> salesAnalysisList;

    public ComparisonDate SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    public List<SalesAnalysis> SalesAnalysisList
    {
        get => this.salesAnalysisList;
        set => this.SetProperty(ref this.salesAnalysisList, value);
    }

    public List<ComparisonDate> ComparisonDates
    {
        get => this.comparisonDates;
        set => this.SetProperty(ref this.comparisonDates, value);
    }

    public override async Task Initialise(CancellationToken cancellationToken){
        await base.Initialise(cancellationToken);
        // TODO: This list will come  from an api call
        List<ComparisonDate> dates = new List<ComparisonDate>();
        dates.Add(new ComparisonDate(new DateTime(2024,1,12), "Yesterday"));
        dates.Add(new ComparisonDate(new DateTime(2024,1,11), "2024-01-11"));
        dates.Add(new ComparisonDate(new DateTime(2024,1,10), "2024-01-10"));
        this.ComparisonDates = dates;
    }

}

[ExcludeFromCodeCoverage]
public record ComparisonDate(DateTime DateTime, String DisplayText);

[ExcludeFromCodeCoverage]
public record SalesAnalysis(String TodaysValue, String ComparisonValue, String VarianceValue, String MainTitle, String TodaysTitle, String ComparisonTitle, String VarianceTitle, String Icon);

public class ReportsBalanceAnalysisPageViewModel : ExtendedBaseViewModel{
    private readonly IMediator Mediator;
    
    public ReportsBalanceAnalysisPageViewModel(INavigationService navigationService,
                                               IApplicationCache applicationCache,
                                               IDialogService dialogService,
                                               IDeviceService deviceService,
                                               IMediator mediator,
                                               INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService, Orientation.Landscape)
    {
        this.Mediator = mediator;
        this.Title = "Balance Analysis";
    }

    public async Task Initialise(CancellationToken cancellationToken){
        await base.Initialise(cancellationToken);

        DateTimeAxis axis1 = new DateTimeAxis(TimeSpan.FromDays(1), (d) => d.ToString("dd/MM/yyyy"));
        this.XAxes = new List<ICartesianAxis>{
                                                       axis1
                                                   };

        Axis axis2 = new Axis();
        axis2.Labeler = Labelers.Currency;

        this.YAxes = new List<ICartesianAxis>{
                                                       axis2
                                                   };

        this.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.CompareOnlyX;
        this.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Top;

        this.Series = new ISeries[]{
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

    public TooltipPosition TooltipPosition{ get; set; }

    public List<ICartesianAxis> YAxes{ get; set; }

    public List<ICartesianAxis> XAxes{ get; set; }

    public ISeries[] Series { get; set; }

    public TooltipFindingStrategy TooltipFindingStrategy { get; set; }


}