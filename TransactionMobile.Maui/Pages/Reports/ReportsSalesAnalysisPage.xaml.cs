using TransactionMobile.Maui.Pages.Common;

namespace TransactionMobile.Maui.Pages.Reports;

using BusinessLogic.ViewModels.Reports;

public partial class ReportsSalesAnalysisPage : ContentPage
{
    private ReportsSalesAnalysisPageViewModel viewModel => this.BindingContext as ReportsSalesAnalysisPageViewModel;

    public ReportsSalesAnalysisPage(ReportsSalesAnalysisPageViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
        this.ComparisonDate.SelectedIndex = 0; // Yesterday
    }
}