using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.MyAccount;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels;

public class ExtendedBaseViewModel : BaseViewModel
{
    #region Fields

    protected readonly IApplicationCache ApplicationCache;

    protected readonly IDialogService DialogService;

    protected readonly INavigationService NavigationService;

    protected readonly IDeviceService DeviceService;
    protected readonly INavigationParameterService NavigationParameterService;

    #endregion

    #region Constructors

    public ExtendedBaseViewModel(IApplicationCache applicationCache,
                                 IDialogService dialogService,
                                 INavigationService navigationService,
                                 IDeviceService deviceService,
                                 INavigationParameterService navigationParameterService,
        Orientation orientation = Orientation.Portrait) {
        this.NavigationService = navigationService;
        this.DeviceService = deviceService;
        this.NavigationParameterService = navigationParameterService;
        this.ApplicationCache = applicationCache;
        this.DialogService = dialogService;
        this.BackButtonCommand = new AsyncCommand(this.BackButtonCommandExecute);
        this.Orientation = orientation;
    }

    #endregion

    #region Properties

    public ICommand BackButtonCommand { get; }

    public virtual async Task Initialise(CancellationToken cancellationToken) {
        this.DeviceService.SetOrientation(this.Orientation);
    }

    public Orientation Orientation { get; private set; }

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