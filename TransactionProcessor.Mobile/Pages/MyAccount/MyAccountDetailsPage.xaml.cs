using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;

namespace TransactionProcessor.Mobile.Pages.MyAccount;

public partial class MyAccountDetailsPage : ContentPage
{
    #region Constructors

    public MyAccountDetailsPage(MyAccountDetailsPageViewModel vm) {
        this.InitializeComponent();

        this.BindingContext = vm;
    }

    #endregion

    #region Properties

    private MyAccountDetailsPageViewModel viewModel => this.BindingContext as MyAccountDetailsPageViewModel;

    #endregion

    #region Methods

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }

    #endregion
}