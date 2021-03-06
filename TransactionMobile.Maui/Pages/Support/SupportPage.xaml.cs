namespace TransactionMobile.Maui.Pages.Support;

using BusinessLogic.ViewModels.Support;

public partial class SupportPage : ContentPage
{
    private SupportPageViewModel viewModel => BindingContext as SupportPageViewModel;

    public SupportPage(SupportPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        SupportPageViewModel vm = MauiProgram.Container.Services.GetRequiredService<SupportPageViewModel>();
        BindingContext = vm;
    }
}