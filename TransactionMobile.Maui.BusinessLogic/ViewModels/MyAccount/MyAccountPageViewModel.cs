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

        private readonly IMemoryCacheService MemoryCacheService;

        #region Constructors

        public MyAccountPageViewModel(INavigationService navigationService,
                                      IMemoryCacheService memoryCacheService)
        {
            this.NavigationService = navigationService;
            this.MemoryCacheService = memoryCacheService;
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
            this.MemoryCacheService.Set<String>("AccessToken", null);
            
            await this.NavigationService.GoToLoginPage();
        }
        
        #endregion
    }
}