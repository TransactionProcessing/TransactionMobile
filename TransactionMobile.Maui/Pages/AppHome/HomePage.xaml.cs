namespace TransactionMobile.Maui.Pages.AppHome;

using BusinessLogic.ViewModels;

public partial class HomePage : ContentPage
{
    #region Constructors

    public HomePage(HomePageViewModel vm) {
        this.InitializeComponent();
        this.BindingContext = vm;
    }

    #endregion

    #region Properties

    private HomePageViewModel viewModel => this.BindingContext as HomePageViewModel;

    #endregion

    #region Methods

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }

    #endregion
}