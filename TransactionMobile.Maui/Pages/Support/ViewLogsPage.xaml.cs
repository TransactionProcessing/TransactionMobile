using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace TransactionMobile.Maui.Pages.Support
{
    using BusinessLogic.ViewModels.Support;

    public partial class ViewLogsPage : ContentPage
    {
        private ViewLogsPageViewModel viewModel => BindingContext as ViewLogsPageViewModel;
        
        public ViewLogsPage(ViewLogsPageViewModel vm) {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing() {
            ViewLogsPageViewModel vm = MauiProgram.Container.Services.GetRequiredService<ViewLogsPageViewModel>();
            await vm.LoadLogMessages();
            BindingContext = vm;
        }
    }
}