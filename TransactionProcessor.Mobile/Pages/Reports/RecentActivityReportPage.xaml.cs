using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class RecentActivityReportPage : ContentPage
{
    private RecentActivityReportPageViewModel viewModel => this.BindingContext as RecentActivityReportPageViewModel;
    private bool hasInitialised;

    public RecentActivityReportPage(RecentActivityReportPageViewModel vm)
    {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (this.hasInitialised)
        {
            return;
        }

        this.hasInitialised = true;
        await this.viewModel.Initialise(CancellationToken.None);
    }
}
