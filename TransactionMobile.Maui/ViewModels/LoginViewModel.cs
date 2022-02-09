﻿namespace TransactionMobile.Maui.ViewModels
{
    using System.Windows.Input;
    using BusinessLogic.Models;
    using BusinessLogic.Requests;
    using MediatR;
    using MvvmHelpers;
    using MvvmHelpers.Commands;

    public class LoginPageViewModel : BaseViewModel
    {
        #region Constructors

        //public String Username { get; set; }
        //public String Password { get; set; }
        public LoginPageViewModel(IMediator mediator)
        {
            this.LoginCommand = new AsyncCommand(this.LoginCommandExecute);
            this.Mediator = mediator;
        }

        #endregion

        #region Properties

        public ICommand LoginCommand { get; }

        public IMediator Mediator { get; }

        #endregion

        #region Methods

        private async Task LoginCommandExecute()
        {
            // TODO: this method needs refactored

            LoginRequest loginRequest = LoginRequest.Create("", "");

            String token = await this.Mediator.Send(loginRequest);

            //if (token == null)
            //{
            //    // TODO: Some kind of error handling
            //}

            // TODO: Logon Transaction
            LogonTransactionRequest logonTransactionRequest = LogonTransactionRequest.Create(DateTime.Now, "1", "", "");
            Boolean logonSuccessful = await this.Mediator.Send(logonTransactionRequest);

            // TODO: get these values off the logon response (maybe make response a tuple)
            App.EstateId = Guid.Parse("56CEE156-6815-4562-A96E-9389C16FA79B");
            App.MerchantId = Guid.Parse("E746EACB-4E73-4E78-B732-53B9C65E5BDA");

            // TODO: Get Contracts & Balance ??
            GetContractProductsRequest getContractProductsRequest = GetContractProductsRequest.Create("", App.EstateId, App.MerchantId);

            // TODO: Cache the result, but will add this to a timer call to keep up to date...
            List<ContractProductModel> products = await this.Mediator.Send(getContractProductsRequest);

            // TODO: Cache the result, but will add this to a timer call to keep up to date...
            GetMerchantBalanceRequest getMerchantBalanceRequest = GetMerchantBalanceRequest.Create("", App.EstateId, App.MerchantId);
            var merchantBalance = await this.Mediator.Send(getMerchantBalanceRequest);

            // TODO: Cache the token as will be needed later
            await Shell.Current.GoToAsync("//home");
        }

        #endregion
    }
}