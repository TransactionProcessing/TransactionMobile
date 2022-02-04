namespace TransactionMobile.Maui.ViewModels
{
    using System.Windows.Input;
    using BusinessLogic.Requests;
    using BusinessLogic.Services;
    using MediatR;
    using MvvmHelpers;
    using MvvmHelpers.Commands;

    public class LoginViewModel : BaseViewModel
    {
        #region Constructors

        //public String Username { get; set; }
        //public String Password { get; set; }
        public LoginViewModel(IMediator mediator)
        {
            this.LoginCommand = new AsyncCommand(this.LoginCommandExecute);
            this.Mediator = mediator;
        }

        #endregion

        #region Properties

        public IAuthenticationService AuthenticationService { get; }

        public ICommand LoginCommand { get; set; }

        public IMediator Mediator { get; }

        #endregion

        #region Methods

        internal async Task InitializeAsync()
        {
        }

        private async Task LoginCommandExecute()
        {
            LoginRequest request = new LoginRequest();

            String token = await this.Mediator.Send(request);

            //if (token == null)
            //{
            //    // TODO: Some kind of error handling
            //}

            // TODO: Cache the token as will be needed later
            await Shell.Current.GoToAsync("//home");
        }

        #endregion
    }
}