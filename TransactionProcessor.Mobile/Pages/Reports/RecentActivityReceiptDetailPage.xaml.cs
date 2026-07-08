using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class RecentActivityReceiptDetailPage : ContentPage
{
    private RecentActivityReceiptDetailPageViewModel viewModel => this.BindingContext as RecentActivityReceiptDetailPageViewModel;

    public RecentActivityReceiptDetailPage(RecentActivityReceiptDetailPageViewModel vm)
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
