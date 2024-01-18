namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Reports;

using Maui.UIServices;
using MediatR;
using Services;
using System.Collections.Generic;
using Common;
using Models;
using MvvmHelpers.Commands;
using UIServices;
using System.Windows.Input;

public class ReportsSalesAnalysisPageViewModel : ExtendedBaseViewModel{

    private readonly IMediator Mediator;
    public ReportsSalesAnalysisPageViewModel(INavigationService navigationService,
                                             IApplicationCache applicationCache,
                                             IDialogService dialogService,
                                             IDeviceService deviceService,
                                             IMediator mediator) : base(applicationCache, dialogService, navigationService, deviceService)
    {
        this.Mediator = mediator;
        this.ComparisonDatePickerSelectedIndexChangedCommand = new AsyncCommand<ComparisonDate>(this.ComparisonDatePickerSelectedExecute);
        this.Title = "Sales Analysis";
    }

    public ICommand ComparisonDatePickerSelectedIndexChangedCommand { get; }

    private async Task ComparisonDatePickerSelectedExecute(ComparisonDate selectedDate){
        // TODO: we will re fire the api calls here on a date change
        await this.GetApiData();
    }

    private async Task GetApiData(){
        // TODO: Initial api call to get data would be done here
        //this.SalesCountAnalysis = new SalesCountAnalysis(100, 98, 0.02m);
        //this.SalesValueAnalysis = new SalesValueAnalysis(1000, 960, 0.04m);
        var salesAnalysisList = new List<SalesAnalysis>();
        salesAnalysisList.Add(new SalesAnalysis("100.00 KES", "90.00 KES", "10%", "Sales Value", "Today's Sales", SelectedItem.DisplayText, "Variance:","salesvalue.svg"));
        salesAnalysisList.Add(new SalesAnalysis("100", "90", "10%", "Sales Count", "Today's Sales", SelectedItem.DisplayText, "Variance:", "salescount.svg"));
        this.SalesAnalysisList = salesAnalysisList;
    }

    private SalesValueAnalysis salesValueAnalysis;

    private SalesCountAnalysis salesCountAnalysis;

    private List<ComparisonDate> comparisonDates;

    ComparisonDate _selectedItem;

    private List<SalesAnalysis> salesAnalysisList;

    public ComparisonDate SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    public SalesValueAnalysis SalesValueAnalysis
    {
        get => this.salesValueAnalysis;
        set => this.SetProperty(ref this.salesValueAnalysis, value);
    }

    public SalesCountAnalysis SalesCountAnalysis
    {
        get => this.salesCountAnalysis;
        set => this.SetProperty(ref this.salesCountAnalysis, value);
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

public record ComparisonDate(DateTime DateTime, String DisplayText);
public record SalesValueAnalysis(Decimal TodaysValue, Decimal ComparisonValue, Decimal Variance);
public record SalesCountAnalysis(Int32 TodaysCount, Decimal ComparisonCount, Decimal Variance);

public record SalesAnalysis(String TodaysValue, String ComparisonValue, String VarianceValue, String MainTitle, String TodaysTitle, String ComparisonTitle, String VarianceTitle, String Icon);

public class ReportsBalanceAnalysisPageViewModel : ExtendedBaseViewModel{
    private readonly IMediator Mediator;

    public ReportsBalanceAnalysisPageViewModel(INavigationService navigationService,
                                               IApplicationCache applicationCache,
                                               IDialogService dialogService,
                                               IDeviceService deviceService,
                                               IMediator mediator) : base(applicationCache, dialogService, navigationService, deviceService, DisplayOrientation.Landscape)
    {
        this.Mediator = mediator;
        //this.ComparisonDatePickerSelectedIndexChangedCommand = new AsyncCommand<ComparisonDate>(this.ComparisonDatePickerSelectedExecute);
        this.Title = "Balance Analysis";
    }

    public async Task Initialise(CancellationToken cancellationToken){
        await base.Initialise(cancellationToken);
    }
}