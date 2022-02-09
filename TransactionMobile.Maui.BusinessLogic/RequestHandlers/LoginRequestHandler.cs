namespace TransactionMobile.Maui.BusinessLogic.RequestHandlers
{
    using MediatR;
    using Requests;
    using Services;

    public class LoginRequestHandler : IRequestHandler<LoginRequest, String>
    {
        #region Constructors
        public LoginRequestHandler(IAuthenticationService authenticationService)
        {
            
            this.AuthenticationService = authenticationService;
        }

        #endregion

        #region Properties

        public IAuthenticationService AuthenticationService { get; }

        #endregion

        #region Methods

        public async Task<String> Handle(LoginRequest request,
                                         CancellationToken cancellationToken)
        {
            var token = await this.AuthenticationService.GetToken(request.UserName, request.Password, cancellationToken);

            return token;
        }

        #endregion

        
    }
}