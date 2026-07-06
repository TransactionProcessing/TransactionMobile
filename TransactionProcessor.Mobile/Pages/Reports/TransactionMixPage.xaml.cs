using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.Pages.Reports;

public partial class TransactionMixPage : ContentPage
{
    private TransactionMixPageViewModel viewModel => this.BindingContext as TransactionMixPageViewModel;

    public TransactionMixPage(TransactionMixPageViewModel vm)
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
