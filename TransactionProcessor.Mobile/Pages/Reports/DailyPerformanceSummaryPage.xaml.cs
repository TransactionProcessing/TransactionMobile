using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class DailyPerformanceSummaryPage : ContentPage
{
    private DailyPerformanceSummaryPageViewModel viewModel => this.BindingContext as DailyPerformanceSummaryPageViewModel;

    public DailyPerformanceSummaryPage(DailyPerformanceSummaryPageViewModel vm)
    {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }
}
