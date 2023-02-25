namespace TransactionMobile.Maui.BusinessLogic.ViewModels;

using System.Windows.Input;
using Logging;
using Maui.UIServices;
using MvvmHelpers;
using MvvmHelpers.Commands;
using MyAccount;
using RequestHandlers;
using Services;
using Support;
using Transactions;
using UIServices;

public class ExtendedBaseViewModel : BaseViewModel
{
    #region Fields

    protected readonly IApplicationCache ApplicationCache;

    protected readonly IDialogService DialogService;

    protected readonly INavigationService NavigationService;

    #endregion

    #region Constructors

    public ExtendedBaseViewModel(IApplicationCache applicationCache,
                                 IDialogService dialogService,
                                 INavigationService navigationService) {
        this.NavigationService = navigationService;
        this.ApplicationCache = applicationCache;
        this.DialogService = dialogService;
        this.BackButtonCommand = new AsyncCommand(this.BackButtonCommandExecute);
    }

    #endregion

    #region Properties

    public ICommand BackButtonCommand { get; }

    #endregion

    #region Methods

    protected async Task BackButtonCommandExecute() {
        Type type = this.GetType().UnderlyingSystemType;
        Task t = type.Name switch {
            nameof(TransactionsPageViewModel) => this.ShowHomePage(),
            nameof(MyAccountPageViewModel) => this.ShowHomePage(),
            nameof(SupportPageViewModel) => this.ShowHomePage(),
            nameof(HomePageViewModel) => this.ShowLoginPage(),
            nameof(LoginPageViewModel) => new Task(() => Application.Current.Quit()),
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
            
            var x = this.ApplicationCache.GetIsLoggedIn();

            await this.NavigationService.GoToLoginPage();
        }
    }

    public virtual void HandleResult<T>(Result<T> result)
    {
        if (result == null) {
            ErrorResult<T> errorResult = new ErrorResult<T>("Result from function call was null");
            throw new ApplicationException(errorResult.Message);
        }

        if (result.Failure)
        {
            ErrorResult<T> errorResult = (ErrorResult<T>)result;
            throw new ApplicationException(errorResult.Message);
        }
        // Success so carry on
    }

    #endregion
}