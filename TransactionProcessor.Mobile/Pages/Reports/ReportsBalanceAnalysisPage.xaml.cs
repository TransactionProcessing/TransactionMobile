using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class ReportsBalanceAnalysisPage : ContentPage
{
    private ReportsBalanceAnalysisPageViewModel viewModel => this.BindingContext as ReportsBalanceAnalysisPageViewModel;

    public ReportsBalanceAnalysisPage(ReportsBalanceAnalysisPageViewModel vm)
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
