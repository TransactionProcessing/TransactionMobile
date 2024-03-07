namespace TransactionMobile.Maui.BusinessLogic.ViewModels;

using System.Windows.Input;
using Common;
using Logging;
using Maui.UIServices;
using MvvmHelpers;
using MvvmHelpers.Commands;
using MyAccount;
using Reports;
using RequestHandlers;
using Services;
using SimpleResults;
using Support;
using Transactions;
using UIServices;

public class ExtendedBaseViewModel : BaseViewModel
{
    #region Fields

    protected readonly IApplicationCache ApplicationCache;

    protected readonly IDialogService DialogService;

    protected readonly INavigationService NavigationService;

    protected readonly IDeviceService DeviceService;

    #endregion

    #region Constructors

    public ExtendedBaseViewModel(IApplicationCache applicationCache,
                                 IDialogService dialogService,
                                 INavigationService navigationService,
                                 IDeviceService deviceService,
                                 DisplayOrientation orientation = DisplayOrientation.Portrait) {
        this.NavigationService = navigationService;
        this.DeviceService = deviceService;
        this.ApplicationCache = applicationCache;
        this.DialogService = dialogService;
        this.BackButtonCommand = new AsyncCommand(this.BackButtonCommandExecute);
        this.Orientation = orientation;
        //this.DeviceService.SetOrientation(orientation);
    }

    #endregion

    #region Properties

    public ICommand BackButtonCommand { get; }

    //public void SetOrientation(DisplayOrientation orientation){
    //    this.
    //}

    public virtual async Task Initialise(CancellationToken cancellationToken){
        this.DeviceService.SetOrientation(this.Orientation);
    }

    public DisplayOrientation Orientation{ get; private set; }

    #endregion

    #region Methods

    protected async Task BackButtonCommandExecute() {
        Type type = this.GetType().UnderlyingSystemType;
        Task t = type.Name switch {
            nameof(TransactionsPageViewModel) => this.ShowHomePage(),
            nameof(MyAccountPageViewModel) => this.ShowHomePage(),
            nameof(ReportsPageViewModel) => this.ShowHomePage(),
            nameof(SupportPageViewModel) => this.ShowHomePage(),
            nameof(HomePageViewModel) => this.ShowLoginPage(),
            nameof(LoginPageViewModel) => this.NavigationService.QuitApplication(),
            _ => this.NavigationService.GoBack()
        };
        await t;
    }

    private async Task ShowHomePage() {
        await this.NavigationService.GoToHome();
    }

    private async Task ShowLoginPage() {

        Boolean leave = await this.DialogService.ShowDialog("Title", "Logout Message", "yes", "no");
        if (leave) {
            Logger.LogInformation("LogoutCommand called");
            this.ApplicationCache.SetIsLoggedIn(false);
            this.ApplicationCache.SetAccessToken(null);
          
            await this.NavigationService.GoToLoginPage();
        }
    }

    public virtual void HandleResult<T>(Result<T> result)
    {
        if (result == null) {
            throw new ApplicationException("Result from function call was null");
        }

        if (result.IsFailed)
        {
            throw new ApplicationException(result.Message);
        }
        // Success so carry on
    }

    #endregion
}