using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;

namespace TransactionProcessor.Mobile.Pages.Support
{
    public partial class ViewLogsPage : ContentPage
    {
        private ViewLogsPageViewModel viewModel => this.BindingContext as ViewLogsPageViewModel;
        
        public ViewLogsPage(ViewLogsPageViewModel vm) {
            this.InitializeComponent();
            this.BindingContext = vm;
        }

        protected override async void OnAppearing() {
            await this.viewModel.LoadLogMessages();
        }
    }
}
