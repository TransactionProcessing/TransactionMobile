using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionMobile.Maui.Requests;
using TransactionMobile.Maui.Services;

namespace TransactionMobile.Maui.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, String>
    {
        public LoginRequestHandler(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        public IAuthenticationService AuthenticationService { get; }

        public async Task<string> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var token = await this.AuthenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return token;
        }
    }
}