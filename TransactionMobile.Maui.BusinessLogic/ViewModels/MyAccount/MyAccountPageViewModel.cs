namespace TransactionMobile.Maui.BusinessLogic.ViewModels.MyAccount
{
    using System.Windows.Input;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using TransactionMobile.Maui.BusinessLogic.Services;
    using TransactionMobile.Maui.UIServices;

    public class MyAccountPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        private readonly IApplicationCache ApplicationCache;

        #region Constructors

        public MyAccountPageViewModel(INavigationService navigationService,IApplicationCache applicationCache)
        {
            this.NavigationService = navigationService;
            this.ApplicationCache = applicationCache;
            this.LogoutCommand = new AsyncCommand(this.LogoutCommandExecute);
            this.Title = "My Account";
        }

        #endregion

        #region Properties

        public ICommand LogoutCommand { get; set; }
        
        #endregion

        #region Methods

        private async Task LogoutCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("LogoutCommand called");
            this.ApplicationCache.SetAccessToken(null);
            
            await this.NavigationService.GoToLoginPage();
        }
        
        #endregion
    }
}