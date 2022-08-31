namespace TransactionMobile.Maui.Pages.Support
{
    using BusinessLogic.ViewModels.Support;
    using TransactionMobile.Maui.Pages.Common;

    public partial class SupportPage : NoBackWithoutLogoutPage
    {
        private SupportPageViewModel viewModel => BindingContext as SupportPageViewModel;

        public SupportPage(SupportPageViewModel vm) {
            InitializeComponent();
            BindingContext = vm;
        }

        //protected override async void OnAppearing() {
            //SupportPageViewModel vm = MauiProgram.Container.Services.GetRequiredService<SupportPageViewModel>();
        //}
    }
}