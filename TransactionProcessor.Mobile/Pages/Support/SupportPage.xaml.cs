using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.Support
{
    public partial class SupportPage : NoBackWithoutLogoutPage
    {
        private SupportPageViewModel viewModel => BindingContext as SupportPageViewModel;

        public SupportPage(SupportPageViewModel vm) {
            this.InitializeComponent();
            BindingContext = vm;
        }

        //protected override async void OnAppearing() {
            //SupportPageViewModel vm = MauiProgram.Container.Services.GetRequiredService<SupportPageViewModel>();
        //}
    }
}