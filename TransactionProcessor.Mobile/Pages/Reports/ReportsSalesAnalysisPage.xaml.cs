using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class ReportsSalesAnalysisPage : ContentPage
{
    private ReportsSalesAnalysisPageViewModel viewModel => this.BindingContext as ReportsSalesAnalysisPageViewModel;

    public ReportsSalesAnalysisPage(ReportsSalesAnalysisPageViewModel vm)
	{
		this.InitializeComponent();

        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.ComparisonDate.SelectedIndex = 0; // Yesterday
    }
}