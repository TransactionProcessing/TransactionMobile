namespace TransactionMobile.Maui.Pages.MyAccount;

using BusinessLogic.ViewModels.MyAccount;

public partial class MyAccountContactPage : ContentPage
{
    #region Constructors

    public MyAccountContactPage(MyAccountContactPageViewModel vm) {
        this.InitializeComponent();

        this.BindingContext = vm;
    }

    #endregion

    #region Properties

    private MyAccountContactPageViewModel viewModel => this.BindingContext as MyAccountContactPageViewModel;

    #endregion

    #region Methods

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.viewModel.Initialise(CancellationToken.None);
    }

    #endregion
}