namespace TransactionMobile.Maui.Pages.AppHome;

using BusinessLogic.Services;
using BusinessLogic.UIServices;
using BusinessLogic.ViewModels;
using Common;
using UIServices;

public partial class HomePage : NoBackWithoutLogoutPage
{
    #region Constructors

    public HomePage(HomePageViewModel vm,
                    IDialogService dialogService,
                    IApplicationCache applicationCache,
                    INavigationService navigationService) {
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