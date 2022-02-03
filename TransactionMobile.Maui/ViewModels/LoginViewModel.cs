using MediatR;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TransactionMobile.Maui.Requests;
using TransactionMobile.Maui.Services;

namespace TransactionMobile.Maui.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {       
        public ICommand LoginCommand { get; set; }
        //public String Username { get; set; }
        //public String Password { get; set; }
        public LoginViewModel(IMediator mediator)
        {
            
            this.LoginCommand = new AsyncCommand(LoginCommandExecute);
            Mediator = mediator;
        }

        public IAuthenticationService AuthenticationService { get; }
        public IMediator Mediator { get; }

        internal async Task InitializeAsync()
        {
            

        }

        private async Task LoginCommandExecute()
        {
            LoginRequest request = new LoginRequest();

            var token = await this.Mediator.Send(request);

            //if (token == null)
            //{
            //    // TODO: Some kind of error handling
            //}

            // TODO: Cache the token as will be needed later
            await Shell.Current.GoToAsync("//home");

        }
    }
}
