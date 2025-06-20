using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels;
using TransactionProcessor.Mobile.Pages.Common;

namespace TransactionProcessor.Mobile.Pages.AppHome;

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